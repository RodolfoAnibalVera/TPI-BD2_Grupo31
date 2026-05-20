using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class RegistrarCuenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as Site;
            if (master != null)
            {
                master.OcultarNavbar();
            }
        }

        protected void btnRegistrarme_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            lblMensajeRegistro.Text = "";

            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            int dni = int.Parse(txtDni.Text.Trim());
            string email = txtEmail.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            string cp = txtCP.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                ClienteNegocio clienteNegocio = new ClienteNegocio();
                int resultado = clienteNegocio.RegistrarCliente(
                    nombre,
                    apellido,
                    dni,
                    email,
                    telefono,
                    direccion,
                    cp,
                    password
                );

                if (resultado == -1)
                {
                    lblMensajeRegistro.Text = "El email ya está en uso.";
                    return;
                }

                if (resultado == -2)
                {
                    lblMensajeRegistro.Text = "El DNI ya está en uso.";
                    return;
                }

                if (resultado <= 0)
                {
                    lblMensajeRegistro.Text = "Ocurrió un error al registrar.";
                    return;
                }

                // Registro OK: solo redirigimos a MiCuenta para que inicie sesión
                Response.Redirect("~/MiCuenta.aspx", false);
            }
            catch
            {
                lblMensajeRegistro.Text = "Ocurrió un error al registrar la cuenta. Intentá nuevamente.";
            }
        }
    }
}