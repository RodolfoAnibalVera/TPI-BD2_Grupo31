using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class RecuperarPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as Site;
            if (master != null)
            {
                master.OcultarNavbar();
            }
        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            lblMensaje.Text = "";

            // Tomamos email y nueva contraseña
            string email = txtEmail.Text.Trim();
            string nuevaPassword = txtNuevaPassword.Text.Trim();

            try
            {
                ClienteNegocio clienteNegocio = new ClienteNegocio();
                bool ok = clienteNegocio.CambiarPasswordPorEmail(email, nuevaPassword);

                // Si se actualizo correctamente
                if (ok)
                {
                    lblMensaje.CssClass = "text-success";
                    lblMensaje.Text = "Contraseña actualizada. Ya podés iniciar sesión.";
                }
                else
                {
                    // Email no encontrado
                    lblMensaje.CssClass = "text-danger";
                    lblMensaje.Text = "No se pudo actualizar la contraseña. Revisá el email.";
                }
            }
            catch
            {
                // Error inesperado
                lblMensaje.CssClass = "text-danger";
                lblMensaje.Text = "Ocurrió un error al actualizar la contraseña. Intentá nuevamente más tarde.";
            }
        }

    }
}