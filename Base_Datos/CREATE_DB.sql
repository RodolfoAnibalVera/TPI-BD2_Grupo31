/* =============================================================
   BOOKSTORE_DB 
   ============================================================= */

CREATE DATABASE BOOKSTORE_DB;
GO

USE BOOKSTORE_DB;
GO

/* =============================================================
   1) TABLAS MAESTRAS
   ============================================================= */

CREATE TABLE TIPOS_USUARIO (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Rol         VARCHAR(50)  NOT NULL UNIQUE,
    Descripcion VARCHAR(150) NULL
);

CREATE TABLE USUARIOS (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario  VARCHAR(100) NOT NULL UNIQUE,
    Contrasena     VARCHAR(255) NOT NULL,
    Email          VARCHAR(150) NOT NULL UNIQUE,
    IdTipoUsuario  INT NOT NULL,
    Activo         BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_Usuarios_TipoUsuario
        FOREIGN KEY (IdTipoUsuario) REFERENCES TIPOS_USUARIO(Id)
);

CREATE TABLE CLIENTES (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Nombre    VARCHAR(100) NOT NULL,
    Apellido  VARCHAR(100) NOT NULL,
    DNI       INT          NOT NULL,
    Email     VARCHAR(150) NOT NULL,
    IdUsuario INT NULL,
    Telefono  VARCHAR(30)  NULL,
    Direccion VARCHAR(200) NULL,
    CP        VARCHAR(10)  NULL,
    CONSTRAINT UQ_Clientes_Email UNIQUE (Email),
    CONSTRAINT UQ_Clientes_DNI   UNIQUE (DNI),
    CONSTRAINT FK_Clientes_Usuario
        FOREIGN KEY (IdUsuario) REFERENCES USUARIOS(Id)
);

CREATE TABLE EDITORIALES (
    Id     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Pais   VARCHAR(50)  NULL
);

CREATE TABLE AUTORES (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Nombre       VARCHAR(100) NOT NULL,
    Nacionalidad VARCHAR(50)  NULL
);

CREATE TABLE CATEGORIAS (
    Id     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Activo BIT NOT NULL DEFAULT(1),
    CONSTRAINT UQ_Categorias_Nombre UNIQUE (Nombre)
);

/* =============================================================
   2) LIBROS
   ============================================================= */

CREATE TABLE LIBROS (
    Id                 INT IDENTITY(1,1) PRIMARY KEY,
    Titulo             VARCHAR(200) NOT NULL,
    Descripcion        VARCHAR(MAX) NULL,
    ISBN               VARCHAR(20)  NULL,
    Idioma             VARCHAR(50)  NULL,
    AnioEdicion        INT          NULL,
    Paginas            INT          NULL,
    Stock              INT NOT NULL DEFAULT(0),
    Activo             BIT NOT NULL DEFAULT(1),
    BestSeller         BIT NOT NULL DEFAULT(0),
    PrecioCompra       DECIMAL(12,2) NULL,
    PrecioVenta        DECIMAL(12,2) NULL,
    PorcentajeGanancia DECIMAL(5,2)  NULL,
    ImagenUrl          VARCHAR(400) NULL,
    IdEditorial        INT NULL,
    IdAutor            INT NULL,
    IdCategoria        INT NOT NULL,
    CONSTRAINT UQ_Libros_ISBN UNIQUE (ISBN),
    CONSTRAINT FK_Libros_Categoria
        FOREIGN KEY (IdCategoria) REFERENCES CATEGORIAS(Id),
    CONSTRAINT FK_Libros_Editorial
        FOREIGN KEY (IdEditorial) REFERENCES EDITORIALES(Id),
    CONSTRAINT FK_Libros_Autor
        FOREIGN KEY (IdAutor) REFERENCES AUTORES(Id)
);

/* =============================================================
   3) CARRITOS
   ============================================================= */

CREATE TABLE CARRITOS (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente   INT NULL,
    CookieId    VARCHAR(100) NULL,
    Creado      DATETIME2(0) NOT NULL DEFAULT (SYSUTCDATETIME()),
    Actualizado DATETIME2(0) NOT NULL DEFAULT (SYSUTCDATETIME()),
    Activo      BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_Carritos_Cliente
        FOREIGN KEY (IdCliente) REFERENCES CLIENTES(Id)
);

CREATE TABLE CARRITO_ITEMS (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    IdCarrito      INT NOT NULL,
    IdLibro        INT NOT NULL,
    Cantidad       INT NOT NULL,
    PrecioUnitario DECIMAL(12,2) NOT NULL,
    CONSTRAINT UQ_Carrito_Items UNIQUE (IdCarrito, IdLibro),
    CONSTRAINT CK_CarritoItems_Cantidad_Pos CHECK (Cantidad > 0),
    CONSTRAINT CK_CarritoItems_Precio_Pos   CHECK (PrecioUnitario >= 0),
    CONSTRAINT FK_CarritoItems_Carrito
        FOREIGN KEY (IdCarrito) REFERENCES CARRITOS(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CarritoItems_Libro
        FOREIGN KEY (IdLibro)   REFERENCES LIBROS(Id)
);

/* =============================================================
   4) PEDIDOS Y RELACIONADAS
   ============================================================= */

CREATE TABLE PEDIDOS (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    NumeroPedido   VARCHAR(30) NOT NULL,
    Fecha          DATETIME2(0) NOT NULL DEFAULT (SYSUTCDATETIME()),
    Estado         VARCHAR(30) NOT NULL,
    Subtotal       DECIMAL(12,2) NOT NULL DEFAULT(0),
    Total          DECIMAL(12,2) NOT NULL DEFAULT(0),
    IdCliente      INT NULL,
    DireccionEnvio VARCHAR(200) NULL,
    CONSTRAINT UQ_Pedidos_Numero UNIQUE (NumeroPedido),
    CONSTRAINT FK_Pedidos_Cliente
        FOREIGN KEY (IdCliente) REFERENCES CLIENTES(Id)
);

CREATE TABLE PEDIDOS_DETALLE (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido       INT NOT NULL,
    IdLibro        INT NOT NULL,
    Cantidad       INT NOT NULL,
    PrecioUnitario DECIMAL(12,2) NOT NULL,
    CONSTRAINT CK_PedidoDet_Cantidad_Pos CHECK (Cantidad > 0),
    CONSTRAINT CK_PedidoDet_Precio_Pos   CHECK (PrecioUnitario >= 0),
    CONSTRAINT FK_PedidoDet_Pedido
        FOREIGN KEY (IdPedido) REFERENCES PEDIDOS(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PedidoDet_Libro
        FOREIGN KEY (IdLibro)  REFERENCES LIBROS(Id)
);

CREATE TABLE PAGOS (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido   INT NOT NULL,
    Monto      DECIMAL(12,2) NOT NULL,
    Metodo     VARCHAR(30) NOT NULL,
    Estado     VARCHAR(30) NOT NULL,
    Referencia VARCHAR(100) NULL,
    Fecha      DATETIME2(0) NOT NULL DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT CK_Pagos_Monto_Pos CHECK (Monto >= 0),
    CONSTRAINT FK_Pagos_Pedido
        FOREIGN KEY (IdPedido) REFERENCES PEDIDOS(Id) ON DELETE CASCADE
);

CREATE TABLE ENVIOS (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido       INT NOT NULL,
    MetodoDeEnvio  VARCHAR(50)  NOT NULL,
    Fecha          DATETIME2(0) NOT NULL DEFAULT (SYSUTCDATETIME()),
    Precio         DECIMAL(12,2) NOT NULL DEFAULT(0),
    EstadoEnvio    VARCHAR(30)  NOT NULL,
    Observaciones  VARCHAR(300) NULL,
    Barrio         VARCHAR(100) NULL,
    Ciudad         VARCHAR(100) NULL,
    Departamento   VARCHAR(100) NULL,
    NombreEnvio    VARCHAR(100) NULL,
    ApellidoEnvio  VARCHAR(100) NULL,
    CPEnvio        VARCHAR(20)  NULL,
    DireccionEnvio VARCHAR(200) NULL,
    CONSTRAINT UQ_Envios_IdPedido UNIQUE (IdPedido),
    CONSTRAINT FK_Envios_Pedido
        FOREIGN KEY (IdPedido) REFERENCES PEDIDOS(Id) ON DELETE CASCADE
);

CREATE TABLE FACTURAS (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido  INT NOT NULL,
    Fecha     DATETIME2(0) NOT NULL DEFAULT (SYSUTCDATETIME()),
    Nombre    VARCHAR(100) NOT NULL,
    Apellido  VARCHAR(100) NOT NULL,
    Direccion VARCHAR(200) NOT NULL,
    Barrio    VARCHAR(100) NULL,
    Ciudad    VARCHAR(100) NOT NULL,
    CP        VARCHAR(20)  NOT NULL,
    Depto     VARCHAR(20)  NULL,
    CONSTRAINT UQ_Facturas_IdPedido UNIQUE (IdPedido),
    CONSTRAINT FK_Facturas_Pedido
        FOREIGN KEY (IdPedido) REFERENCES PEDIDOS(Id) ON DELETE CASCADE
);

/* =============================================================
   5) AUDITORIA
   ============================================================= */

CREATE TABLE LOG_ELIMINACION_PEDIDOS (
    IdPedido INT,
    FechaEliminacion DATETIME DEFAULT GETDATE()
);