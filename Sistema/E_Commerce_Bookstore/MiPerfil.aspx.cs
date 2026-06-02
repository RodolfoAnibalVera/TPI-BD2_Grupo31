using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Cliente cliente = Session["Cliente"] as Cliente;

                    if (cliente == null)
                    {
                        Response.Redirect("MiCuenta.aspx?redirect=MiPerfil.aspx");
                        return;
                    }

                    lblSaludo.Text = "¡Hola " + cliente.Nombre + "!";

                    lblNombre.Text = cliente.Nombre;
                    lblApellido.Text = cliente.Apellido;
                    lblDni.Text = cliente.DNI.ToString();
                    lblEmail.Text = cliente.Email;
                    lblTelefono.Text = cliente.Telefono;
                    lblDireccion.Text = cliente.Direccion;
                    lblCP.Text = cliente.CP;
                }
                catch (Exception ex)
                {
                    Session["error"] = ex;
                    Response.Redirect("Error.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }


        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ModificacionCliente.aspx");
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Limpiar sesión
            Session.Clear();
            Session.Abandon();

            // Eliminar cookie del carrito
            if (Request.Cookies["CarritoId"] != null)
            {
                HttpCookie cookie = new HttpCookie("CarritoId");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            // Redirigir
            Response.Redirect("MiCuenta.aspx");
        }
    }
}