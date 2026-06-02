using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class ModificacionCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                Cliente cliente = Session["Cliente"] as Cliente;
                if (cliente == null)
                {
                    Response.Redirect("MiCuenta.aspx?ReturnUrl=ModificacionCliente.aspx");
                    return;
                }

                txtNombre.Text = cliente.Nombre;
                txtApellido.Text = cliente.Apellido;
                txtDNI.Text = cliente.DNI.ToString();
                txtEmail.Text = cliente.Email;
                txtTelefono.Text = cliente.Telefono;
                txtDireccion.Text = cliente.Direccion;
                txtCP.Text = cliente.CP;
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return; // Si falla alguna validación del cliente, no sigue

            Cliente cliente = Session["Cliente"] as Cliente;
            if (cliente == null) return;

            // Validaciones adicionales en servidor
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtCP.Text))
            {
                lblError.Text = "Todos los campos son obligatorios.";
                lblError.Visible = true;
                return;
            }

            if (!int.TryParse(txtDNI.Text, out int dni))
            {
                lblError.Text = "El DNI debe ser numérico.";
                lblError.Visible = true;
                return;
            }

            if (!long.TryParse(txtTelefono.Text, out _))
            {
                lblError.Text = "El teléfono debe ser numérico.";
                lblError.Visible = true;
                return;
            }

            if (!int.TryParse(txtCP.Text, out _))
            {
                lblError.Text = "El código postal debe ser numérico.";
                lblError.Visible = true;
                return;
            }

            // Asignación de valores
            cliente.Nombre = txtNombre.Text.Trim();
            cliente.Apellido = txtApellido.Text.Trim();
            cliente.DNI = dni;
            cliente.Telefono = txtTelefono.Text.Trim();
            cliente.Direccion = txtDireccion.Text.Trim();
            cliente.CP = txtCP.Text.Trim();

            ClienteNegocio negocio = new ClienteNegocio();
            string mensajeError;
            bool ok = negocio.ValidarYActualizarCliente(cliente, out mensajeError);

            if (!ok)
            {
                lblError.Text = mensajeError;
                lblError.Visible = true;
                return;
            }

            Session["Cliente"] = cliente;
            Response.Redirect("MiPerfil.aspx");
        }
    }
}