/*VISTAS*/

USE BOOKSTORE_DB
GO

--VISTA "VW_LibrosDisponibles" QUE ME DEVUELVE TODOS LOS LIBROS QUE ESTEN ACTIVOS
--PARA SER UTILIZADO EN CATALOGO
CREATE VIEW VW_LibrosDisponibles AS
SELECT 
    L.Id,
    L.Titulo,
    L.ISBN,
    L.Stock,
    L.PrecioVenta,
    L.ImagenUrl,
    L.BestSeller,
    A.Id AS IdAutor,
    A.Nombre AS AutorNombre,
    E.Id AS IdEditorial,
    E.Nombre AS EditorialNombre,
    C.Id AS IdCategoria,
    C.Nombre AS CategoriaNombre
FROM LIBROS L
LEFT JOIN AUTORES A ON L.IdAutor = A.Id
LEFT JOIN EDITORIALES E ON L.IdEditorial = E.Id
LEFT JOIN CATEGORIAS C ON L.IdCategoria = C.Id
WHERE L.Activo = 1;
GO

--VISTA "VW_LibrosPorCategoria" PARA SER UTILIZADA EN
--EL FITRO DE CATALOGO POR CATEGORIA

CREATE VIEW VW_LibrosPorCategoria AS
SELECT 
    L.Id,
    L.Titulo,
    L.Descripcion,
    L.ISBN,
    L.Idioma,
    L.AnioEdicion,
    L.Paginas,
    L.Stock,
    L.Activo,
    L.BestSeller,
    L.PrecioCompra,
    L.PrecioVenta,
    L.PorcentajeGanancia,
    L.ImagenUrl,
    C.Id AS IdCategoria,
    C.Nombre AS CategoriaNombre,
    E.Id AS IdEditorial,
    E.Nombre AS EditorialNombre,
    A.Id AS IdAutor,
    A.Nombre AS AutorNombre
FROM LIBROS L
INNER JOIN CATEGORIAS C ON L.IdCategoria = C.Id
LEFT JOIN EDITORIALES E ON L.IdEditorial = E.Id
LEFT JOIN AUTORES A ON L.IdAutor = A.Id
WHERE L.Activo = 1;
GO

--VISTA "VW_LibroDetalle" QUE DEVUELVE EL LIBRO COMPLETO
--PARA SER UTILIZADO EN PANTALLA DE DETALLE DEL LIBRO SELECCIONADO
--QUE YA FUE FILTRADO PREVIAMENTE POR "ACTIVO" EN OTRAS PANTALLAS

CREATE VIEW VW_LibroDetalle AS
SELECT 
    L.Id,
    L.Titulo,
    L.Descripcion,
    L.ISBN,
    L.Idioma,
    L.AnioEdicion,
    L.Paginas,
    L.Stock,
    L.Activo,
    L.PrecioCompra,
    L.PrecioVenta,
    L.PorcentajeGanancia,
    L.ImagenUrl,
    L.BestSeller,
    A.Id AS IdAutor,
    A.Nombre AS NombreAutor,
    A.Nacionalidad,
    E.Id AS IdEditorial,
    E.Nombre AS NombreEditorial,
    E.Pais,
    C.Id AS CategoriaId,
    C.Nombre AS CategoriaNombre
FROM Libros L
INNER JOIN Categorias C ON L.IdCategoria = C.Id
LEFT JOIN Autores A ON L.IdAutor = A.Id
LEFT JOIN Editoriales E ON L.IdEditorial = E.Id;
 