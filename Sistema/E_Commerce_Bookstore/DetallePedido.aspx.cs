using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class DetallePedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string idStr = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(idStr))
            {
                Response.Redirect("MisPedidos.aspx", false);
                return;
            }

            int idPedido;
            if (!int.TryParse(idStr, out idPedido))
            {
                Response.Redirect("MisPedidos.aspx", false);
                return;
            }

            try
            {
                CargarCabecera(idPedido);
                CargarItems(idPedido);

                var master = this.Master as Site;
                if (master != null)
                {
                    master.OcultarNavbar();
                }
            }
            catch
            {
                Response.Redirect("Error.aspx", false);
            }
        }


        private void CargarCabecera(int idPedido)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                                    SELECT 
                                    p.NumeroPedido,p.Fecha,p.Estado,p.Subtotal,
                                    p.Total,e.DireccionEnvio,p.IdCliente
                                    FROM PEDIDOS p
                                    LEFT JOIN ENVIOS e ON e.IdPedido = p.Id
                                    WHERE p.Id = @Id
                                    ");

                datos.setearParametro("@Id", idPedido);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                {
                    Response.Redirect("MisPedidos.aspx", false);
                    return;
                }

                // Validación opcional por seguridad
                if (Session["IdCliente"] != null)
                {
                    int idCli = Convert.ToInt32(Session["IdCliente"]);
                    int idClientePedido = Convert.ToInt32(datos.Lector["IdCliente"]);

                    if (idCli != idClientePedido)
                    {
                        Response.Redirect("MisPedidos.aspx", false);
                        return;
                    }
                }

                lblNumero.Text = datos.Lector["NumeroPedido"].ToString();
                lblFecha.Text = ((DateTime)datos.Lector["Fecha"]).ToString("dd/MM/yyyy HH:mm");
                lblEstado.Text = datos.Lector["Estado"].ToString();
                lblDireccionEnvio.Text = datos.Lector["DireccionEnvio"]?.ToString() ?? "Retiro en local";
                lblTotal.Text = Convert.ToDecimal(datos.Lector["Total"]).ToString("N2", CultureInfo.InvariantCulture);

                // Dirección de envío / retiro en local
                object dirObj = datos.Lector["DireccionEnvio"];
                if (dirObj == DBNull.Value || string.IsNullOrWhiteSpace(dirObj.ToString()))
                    lblDireccionEnvio.Text = "Retiro en local";
                else
                    lblDireccionEnvio.Text = dirObj.ToString();


                // Obtener método de pago (desde tabla PAGOS)
                PedidoNegocio neg = new PedidoNegocio();
                string metodo = neg.ObtenerMetodoPago(idPedido);
                lblPago.Text = metodo;

            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private void CargarItems(int idPedido)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT l.Titulo,
                           d.Cantidad,
                           d.PrecioUnitario,
                           (d.Cantidad * d.PrecioUnitario) AS Subtotal
                    FROM PEDIDOS_DETALLE d
                    INNER JOIN LIBROS l ON l.Id = d.IdLibro
                    WHERE d.IdPedido = @Id
                    ORDER BY l.Titulo");
                datos.setearParametro("@Id", idPedido);
                datos.ejecutarLectura();

                var filas = new List<ItemFila>();
                while (datos.Lector.Read())
                {
                    filas.Add(new ItemFila
                    {
                        Titulo = datos.Lector["Titulo"].ToString(),
                        Cantidad = Convert.ToInt32(datos.Lector["Cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(datos.Lector["PrecioUnitario"]),
                        Subtotal = Convert.ToDecimal(datos.Lector["Subtotal"])
                    });
                }

                gvItems.DataSource = filas;
                gvItems.DataBind();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private class ItemFila
        {
            public string Titulo { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Subtotal { get; set; }
        }
    }
}