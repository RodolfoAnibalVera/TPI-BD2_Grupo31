/* =============================================================
   BOOKSTORE_2_DB 
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
    Estado         VARCHAR(30) NOT NULL,  -- Pendiente / Enviado / Entregado / Cancelado
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
   5) DATOS DE PRUEBA
   ============================================================= */

-- TIPOS_USUARIO
INSERT INTO TIPOS_USUARIO (Rol, Descripcion) VALUES
('Admin',   'Usuario administrador del sistema'),
('Cliente', 'Cliente registrado de la tienda');

-- USUARIOS
INSERT INTO USUARIOS (NombreUsuario, Contrasena, Email, IdTipoUsuario, Activo) VALUES
('nicolas@mail.com',              '1234',     'nicolas@mail.com',    2, 1),
('juan@mail.com',                 '1234',     'juan@mail.com',       2, 1),
('anto@mail.com',                 '1234',     'anto@mail.com',       2, 1),
('martin@bookstore.com',               'admin1234',     'martin@bookstore.com',     1, 1),
('sofia@bookstore.com',                'admin1234',     'sofia@bookstore.com',      1, 1);

-- CLIENTES
INSERT INTO CLIENTES (Nombre, Apellido, DNI, Email, IdUsuario, Telefono, Direccion, CP) VALUES
('Nicolas', 'Strozzi', 30111222, 'nicolas@mail.com', 1, '351-1111111', 'Av. Siempre Viva 123', '5000'),
('Juan',    'Perez',   28999888, 'juan@mail.com',    2, '351-2222222', 'San Martin 456',       '5001'),
('Anto',    'Lopez',   31222333, 'anto@mail.com',    3, '351-3333333', 'Bv. Illia 789',        '5002'),
('Martin',  'Gomez',   30333444, 'martin@mail.com',  4, '351-4444444', 'Av. Colon 1500',       '5003'),
('Sofia',   'Diaz',    32555666, 'sofia@mail.com',   5, '351-5555555', 'Belgrano 900',         '5004');

-- EDITORIALES
INSERT INTO EDITORIALES (Nombre, Pais) VALUES
('Planeta',              'Argentina'),
('Sudamericana',         'Argentina'),
('Penguin Random House', 'Estados Unidos'),
('Prentice Hall', 'EE.UU.');

-- AUTORES
INSERT INTO AUTORES (Nombre, Nacionalidad) VALUES
('Jorge Luis Borges',    'Argentina'),
('Julio Cortazar',       'Argentina'),
('Gabriel Garcia Marquez','Colombia'),
('Isabel Allende',       'Chile'),
('Alan Moore',           'Reino Unido'),
('Stan Lee',             'Estados Unidos'),
('Robert C. Martin', 'EE.UU.');

-- CATEGORIAS (sin acentos)
INSERT INTO CATEGORIAS (Nombre, Activo) VALUES
('Novela',   1),
('Cuento',   1),
('Poesia',   1),
('Comics',   1),
('Ficcion',  1),
('Infantil', 1),
('Fantasia', 1),
('Terror',   1),
('Programacion',   1);

-- LIBROS (10 libros, incluye varios Comics) – VERSION CORREGIDA
INSERT INTO LIBROS (
    Titulo, Descripcion, ISBN, Idioma, AnioEdicion, Paginas, Stock,
    Activo, BestSeller, PrecioCompra, PrecioVenta, PorcentajeGanancia,
    ImagenUrl, IdEditorial, IdAutor, IdCategoria
)
VALUES
('Ficciones', 'Descripcion de Ficciones', '9780000000001', 'Espanol', 2011, 160, 30, 1, 1,
 1500.00, 2500.00, 66.67,
 'https://images.cdn3.buscalibre.com/fit-in/360x360/46/85/4685286dbc1ec2013245afe1d537acfb.jpg',
 1, 1, 2),

('Rayuela', 'Descripcion de Rayuela', '9780000000002', 'Espanol', 2012, 170, 32, 1, 1,
 1920.00, 3200.00, 66.67,
 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQHXP59rXNYy4X5-FrItmNn3cwKCIwahddBlg&s',
 2, 2, 1),

('Cien años de soledad', 'Descripcion de Cien años de soledad', '9780000000003', 'Espanol', 2013, 180, 34, 1, 1,
 2280.00, 3800.00, 66.67,
 'https://www.edicontinente.com.ar/image/titulos/9788466379717.jpg',
 3, 3, 1),

('La casa de los espiritus', 'Descripcion de La casa de los espiritus', '9780000000004', 'Espanol', 2014, 190, 36, 1, 0,
 1800.00, 3000.00, 66.67,
 'https://images.cdn3.buscalibre.com/fit-in/360x360/d9/1c/d91ca2ad0494df2c14e28343847ca2af.jpg',
 1, 4, 1),

('Watchmen', 'Descripcion de Watchmen', '9780000000005', 'Espanol', 2015, 200, 40, 1, 1,
 2700.00, 4500.00, 66.67,
 'https://images.cdn1.buscalibre.com/fit-in/360x360/e3/d8/e3d8e9b9229ead55e155c48bedeba9ae.jpg',
 3, 5, 4),

('Spiderman: Origen', 'Descripcion de Spiderman: Origen', '9780000000006', 'Espanol', 2016, 210, 38, 1, 1,
 2520.00, 4200.00, 66.67,
 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxl3Ge2DDvhT1ECIPy6YpxBhaTqHz0F6hWbA&s',
 3, 6, 4),

('Cuentos de la selva', 'Descripcion de Cuentos de la selva', '9780000000007', 'Espanol', 2017, 220, 28, 1, 0,
 1080.00, 1800.00, 66.67,
 'https://www.educ.ar/uploads/resources/images/cuentos-de-la-selva_20200321170331.jpg',
 2, NULL, 2),

('Poesias completas', 'Descripcion de Poesias completas', '9780000000008', 'Espanol', 2018, 230, 26, 1, 0,
 1260.00, 2100.00, 66.67,
 'https://www.cervantesvirtual.com/images/portales/antonio_machado/graf/04_portadas/1928_antonio_machado_poesias_completas_portada_s.jpg',
 1, 1, 3),

('Fantasia oscura', 'Descripcion de Fantasia oscura', '9780000000009', 'Espanol', 2019, 240, 24, 1, 0,
 1560.00, 2600.00, 66.67,
 'PONE_ACA_LA_URL_REAL',
 1, NULL, 7),

('Noches de terror', 'Descripcion de Noches de terror', '9780000000010', 'Espanol', 2020, 250, 22, 1, 0,
 1620.00, 2700.00, 66.67,
 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRjE77uS2o4kWTxdy2rQA1VjY9HViP3rVsQtA&s',
 2, NULL, 8);

 INSERT INTO LIBROS (Titulo, Descripcion, ISBN, Idioma, AnioEdicion, Paginas, Stock, Activo, BestSeller,
    PrecioCompra, PrecioVenta, PorcentajeGanancia, ImagenUrl, IdEditorial, IdAutor, IdCategoria) VALUES
('Clean Code', 'Buenas prácticas de desarrollo en Java', '9780132350884', 'Inglés', 2008, 464, 15, 1, 1, 4500.00, 7500.00, 40.00, 'https://m.media-amazon.com/images/I/71T7aD3EOTL._UF350,350_QL50_.jpg', 4, 6, 9),
('Clean Architecture', 'Diseño de software escalable y mantenible', '9780134494166', 'Inglés', 2017, 432, 10, 1, 0, 4700.00, 7600.00, 38.00, 'https://http2.mlstatic.com/D_NQ_NP_2X_804323-MLA93494888921_092025-F.webp', 4, 6, 9),
('The Clean Coder', 'Conducta profesional para desarrolladores', '9780137081073', 'Inglés', 2011, 256, 8, 1, 0, 4200.00, 6900.00, 39.00, 'https://http2.mlstatic.com/D_NQ_NP_2X_800971-MLA92980729610_092025-F.webp', 4, 6, 9);


-- CARRITOS
INSERT INTO CARRITOS (IdCliente, CookieId, Creado, Actualizado, Activo) VALUES
(1, 'cookie-nicolas-001', '2025-11-18T09:00:00', '2025-11-18T09:05:00', 1),
(3, 'cookie-anto-001',    '2025-11-20T10:00:00', '2025-11-20T10:10:00', 1);

INSERT INTO CARRITO_ITEMS (IdCarrito, IdLibro, Cantidad, PrecioUnitario) VALUES
(1, 5, 1, 4500.00),
(1, 2, 1, 3200.00),
(2, 6, 1, 4200.00);

-- PEDIDOS (solo estados: Pendiente, Enviado, Entregado, Cancelado)
INSERT INTO PEDIDOS (NumeroPedido, Fecha, Estado, Subtotal, Total, IdCliente, DireccionEnvio) VALUES
('PED001', '2025-11-19T10:00:00', 'Entregado', 12200.00, 12700.00, 1, 'Direccion 1'),
('PED002', '2025-11-20T10:00:00', 'Enviado',   12600.00, 13100.00, 2, 'Direccion 2'),
('PED003', '2025-11-21T10:00:00', 'Pendiente', 10800.00, 11300.00, 3, 'Direccion 3'),
('PED004', '2025-11-22T10:00:00', 'Cancelado',  4200.00,  4700.00, 1, 'Direccion 1'),
('PED005', '2025-11-23T10:00:00', 'Entregado',  7200.00,  7200.00, 5, 'Retiro en local'),
('PED006', '2025-11-24T10:00:00', 'Pendiente',  8400.00,  8900.00, 4, 'Direccion 4'),
('PED007', '2025-11-25T10:00:00', 'Entregado',  8700.00,  9200.00, 2, 'Direccion 2'),
('PED008', '2025-11-26T10:00:00', 'Enviado',    8100.00,  8600.00, 3, 'Direccion 3');

-- PEDIDOS_DETALLE (se ve claramente que Watchmen y Spiderman se venden bastante, y hay Comics s� o s�)
INSERT INTO PEDIDOS_DETALLE (IdPedido, IdLibro, Cantidad, PrecioUnitario) VALUES
(1, 5, 2, 4500.00),   -- Watchmen
(1, 2, 1, 3200.00),   -- Rayuela
(2, 6, 3, 4200.00),   -- Spiderman x3
(3, 5, 1, 4500.00),   -- Watchmen
(3, 3, 1, 3800.00),   -- Cien anos
(3, 1, 1, 2500.00),   -- Ficciones
(4, 6, 1, 4200.00),   -- Spiderman (pedido cancelado)
(5, 7, 4, 1800.00),   -- Cuentos de la selva
(6, 2, 1, 3200.00),   -- Rayuela
(6, 9, 2, 2600.00),   -- Fantasia oscura
(7, 5, 1, 4500.00),   -- Watchmen
(7, 6, 1, 4200.00),   -- Spiderman
(8,10, 3, 2700.00);   -- Noches de terror

-- ENVIOS (uno por pedido, IdPedido unico)
INSERT INTO ENVIOS (IdPedido, MetodoDeEnvio, Fecha, Precio, EstadoEnvio, Observaciones,
                    Barrio, Ciudad, Departamento, NombreEnvio, ApellidoEnvio, CPEnvio, DireccionEnvio)
VALUES
(1, 'Envio a domicilio', '2025-11-19T15:00:00', 500.00, 'Entregado', 'Entrega por la tarde',
 NULL, 'Cordoba', NULL, 'Nicolas', 'Strozzi', '5000', 'Av. Siempre Viva 123'),
(2, 'Envio a domicilio', '2025-11-20T15:00:00', 500.00, 'Enviado', 'Entrega por la tarde',
 NULL, 'Cordoba', NULL, 'Juan', 'Perez', '5001', 'San Martin 456'),
(3, 'Envio a domicilio', '2025-11-21T15:00:00', 500.00, 'Pendiente', 'A coordinar',
 NULL, 'Cordoba', NULL, 'Anto', 'Lopez', '5002', 'Bv. Illia 789'),
(4, 'Envio a domicilio', '2025-11-22T15:00:00', 500.00, 'Cancelado', 'Pedido cancelado',
 NULL, 'Cordoba', NULL, 'Nicolas', 'Strozzi', '5000', 'Av. Siempre Viva 123'),
(5, 'Retiro en local',   '2025-11-23T15:00:00',   0.00, 'Entregado', 'Retiro en mostrador',
 NULL, 'Cordoba', NULL, 'Sofia', 'Diaz', '5004', 'Belgrano 900'),
(6, 'Envio a domicilio', '2025-11-24T15:00:00', 500.00, 'Pendiente', 'A coordinar',
 NULL, 'Cordoba', NULL, 'Martin', 'Gomez', '5003', 'Av. Colon 1500'),
(7, 'Envio a domicilio', '2025-11-25T15:00:00', 500.00, 'Entregado', 'Entrega por la tarde',
 NULL, 'Cordoba', NULL, 'Juan', 'Perez', '5001', 'San Martin 456'),
(8, 'Envio a domicilio', '2025-11-26T15:00:00', 500.00, 'Enviado', 'Entrega por la tarde',
 NULL, 'Cordoba', NULL, 'Anto', 'Lopez', '5002', 'Bv. Illia 789');

-- PAGOS
INSERT INTO PAGOS (IdPedido, Monto, Metodo, Estado, Referencia, Fecha) VALUES
(1, 12700.00, 'Tarjeta de credito', 'Aprobado',  'PAY-0001', '2025-11-19T11:00:00'),
(2, 13100.00, 'Tarjeta de credito', 'Aprobado',  'PAY-0002', '2025-11-20T11:00:00'),
(3, 11300.00, 'Tarjeta de credito', 'Pendiente', 'PAY-0003', '2025-11-21T11:00:00'),
(4,  4700.00, 'Tarjeta de credito', 'Devuelto',  'PAY-0004', '2025-11-22T11:00:00'),
(5,  7200.00, 'Tarjeta de credito', 'Aprobado',  'PAY-0005', '2025-11-23T11:00:00'),
(6,  8900.00, 'Tarjeta de credito', 'Pendiente', 'PAY-0006', '2025-11-24T11:00:00'),
(7,  9200.00, 'Tarjeta de credito', 'Aprobado',  'PAY-0007', '2025-11-25T11:00:00'),
(8,  8600.00, 'Tarjeta de credito', 'Aprobado',  'PAY-0008', '2025-11-26T11:00:00');

-- FACTURAS (solo pedidos Entregado / Enviado)
INSERT INTO FACTURAS (IdPedido, Fecha, Nombre, Apellido, Direccion, Barrio, Ciudad, CP, Depto) VALUES
(1, '2025-11-19T12:00:00', 'Nicolas', 'Strozzi', 'Av. Siempre Viva 123', 'Centro', 'Cordoba', '5000', NULL),
(2, '2025-11-20T12:00:00', 'Juan',    'Perez',   'San Martin 456',       'Centro', 'Cordoba', '5001', NULL),
(5, '2025-11-23T12:00:00', 'Sofia',   'Diaz',    'Belgrano 900',         'Centro', 'Cordoba', '5004', NULL),
(7, '2025-11-25T12:00:00', 'Juan',    'Perez',   'San Martin 456',       'Centro', 'Cordoba', '5001', NULL),
(8, '2025-11-26T12:00:00', 'Anto',    'Lopez',   'Bv. Illia 789',        'Centro', 'Cordoba', '5002', NULL);
