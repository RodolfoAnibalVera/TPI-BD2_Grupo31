/*Funcion de usuario*/

use BOOKSTORE_DB

---Esta función recibe el precio de compra y el porcentaje de ganancia, 
---calcula el precio de venta y devuelve el resultado.

CREATE FUNCTION FN_CalcularPrecioVenta
(
    @PrecioCompra DECIMAL(12,2),
    @PorcentajeGanancia DECIMAL(5,2)
)
RETURNS DECIMAL(12,2)
AS
BEGIN

    DECLARE @PrecioVenta DECIMAL(12,2)

    SET @PrecioVenta =
        @PrecioCompra +
        (@PrecioCompra * @PorcentajeGanancia / 100)

    RETURN @PrecioVenta

END
GO