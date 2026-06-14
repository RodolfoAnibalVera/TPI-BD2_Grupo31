/*TRIGGERS*/

USE BOOKSTORE_DB;
GO

---Los estados 'Entregado' y 'Cancelado' son estados finales. 
---Una vez que un pedido llega a alguno de ellos, no puede volver a modificarse.

CREATE TRIGGER TR_EstadosFinalesPedido
ON PEDIDOS
AFTER UPDATE
AS
BEGIN

    IF EXISTS
    (
        SELECT *
        FROM inserted I
        INNER JOIN deleted D
            ON I.Id = D.Id
        WHERE D.Estado IN ('Entregado', 'Cancelado')
          AND D.Estado <> I.Estado
    )
    BEGIN
        RAISERROR(
            'Un pedido entregado o cancelado no puede cambiar de estado.',
            16,
            1
        );

        ROLLBACK TRANSACTION;
    END

END
GO

--TRIGGER "TR_SoloEliminarPedidosCancelados" AGREGA UNA REGLA DE NEGOCIO A LA TABLA PEDIDOS
--SE DISPARA CUANDO SE INTENTA ELIMINAR UNA O VARIAS FILAS DE LA TABLA PEDIDOS
--EVITA QUE PUEDA ELIMINARSE UN PEDIDO QUE AUN NO FUE CANCELADO
--REVISA LA TABLA VIRTUAL DELETED, SI AL MENOS UNA FILA NO TIENE ESTADO CANCELADO
--LANZA UN RAISERROR QUE PUEDE SER CAPTURADO EN LA APLICACION Y CANCELA LA TRANSACCION HACE ROLLBACK.
--SI EL O LOS PEDIDOS QUE SE INTENTA ELIMINAR ESTAN CON ESTADO CANCELADO EJECUTA DELETE
--SE DISPARA EN LA PANTALLA "GESTION DE VENTAS" CUANDO SE INTENTA ELIMINAR UN PEDIDO Y MUESTRA UN MENSAJE
--QUE SE CAPTURA COMO SQLEXCEPTION
CREATE TRIGGER TR_SoloEliminarPedidosCancelados
ON PEDIDOS
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM DELETED D
        WHERE D.Estado <> 'Cancelado'
    )
    BEGIN
        RAISERROR('Solo se pueden eliminar pedidos en estado Cancelado.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    DELETE FROM PEDIDOS
    WHERE Id IN (SELECT Id FROM DELETED);
END;
GO


--TRIGGER "TR_LogEliminacionPedidos" QUE SE DISPARA TRAS UNA ELIMINACION DE LA TABLA PEDIDOS
--Y HACE UN INSERT DEL ID DE PEDIDO ELIMINADO EN LA TABLA DE LOG_ELIMINACION_PEDIDOS A MODO DE AUDITORIA
--LA FECHA SE INSERTA AUTOMATICAMENTE YA QUE ESTA DEFINIDO COMO DEFAUT FECHA Y HORA ACTUAL EN LA TABLA
CREATE TRIGGER TR_LogEliminacionPedidos
ON PEDIDOS
AFTER DELETE
AS
BEGIN
    INSERT INTO LOG_ELIMINACION_PEDIDOS (IdPedido)
    SELECT Id FROM DELETED;
END;
GO


---El trigger se ejecuta automáticamente después de insertar o modificar un libro. 
---Toma los valores ingresados de PrecioCompra y PorcentajeGanancia y utiliza la función FN_CalcularPrecioVenta 
---para actualizar el PrecioVenta, garantizando que siempre se mantenga consistente.

CREATE TRIGGER TR_CalcularPrecioVenta
ON LIBROS
AFTER INSERT, UPDATE
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE L
    SET PrecioVenta =
        dbo.FN_CalcularPrecioVenta
        (
            I.PrecioCompra,
            I.PorcentajeGanancia
        )
    FROM LIBROS L
    INNER JOIN inserted I
        ON L.Id = I.Id;

END
GO