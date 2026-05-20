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
    public partial class GestionClientes : System.Web.UI.Page
    {
        GestionClienteNegocio negocio = new GestionClienteNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdTipoUsuario"] == null || Session["IdTipoUsuario"].ToString() != "1")
            {
                Session["error"] = new Exception("Acceso denegado: solo administradores pueden ingresar a esta página.");
                Response.Redirect("Error.aspx");
            }

            if (!IsPostBack)
            {
                CargarGrilla();
                RecuperarDatosDesdeUsuario();
            }
        }

        private void RecuperarDatosDesdeUsuario()
        {
            if (Session["EmailCliente"] != null)
            {
                txtEmail.Text = Session["EmailCliente"].ToString();
                Session.Remove("EmailCliente");
            }

            if (Session["IdUsuarioCliente"] != null)
            {
                txtIdUsuario.Text = Session["IdUsuarioCliente"].ToString();
                Session.Remove("IdUsuarioCliente");
            }
        }


        private void CargarGrilla()
        {
            dgvClientes.DataSource = negocio.Listar();
            dgvClientes.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDNI.Text = "";
            txtEmail.Text = "";
            txtIdUsuario.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            txtCP.Text = "";

            lblMensaje.Text = "";
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                negocio.Eliminar(int.Parse(txtId.Text));
                lblMensaje.Text = "<div class='alert alert-danger'>Cliente eliminado.</div>";
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Error: " + ex.Message + "</div>";
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                ClienteGestionDTO mod = new ClienteGestionDTO
                {
                    Id = int.Parse(txtId.Text),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    DNI = int.Parse(txtDNI.Text),
                    Email = txtEmail.Text,
                    IdUsuario = int.Parse(txtIdUsuario.Text),
                    Telefono = txtTelefono.Text,
                    Direccion = txtDireccion.Text,
                    CP = txtCP.Text
                };

                negocio.Modificar(mod);
                lblMensaje.Text = "<div class='alert alert-warning'>Cliente modificado.</div>";
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Error: " + ex.Message + "</div>";
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                ClienteGestionDTO nuevo = new ClienteGestionDTO
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    DNI = int.Parse(txtDNI.Text),
                    Email = txtEmail.Text,
                    IdUsuario = int.Parse(txtIdUsuario.Text),
                    Telefono = txtTelefono.Text,
                    Direccion = txtDireccion.Text,
                    CP = txtCP.Text
                };

                negocio.Agregar(nuevo);
                lblMensaje.Text = "<div class='alert alert-success'>Cliente agregado.</div>";
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Error: " + ex.Message + "</div>";
            }
        }

        protected void dgvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = dgvClientes.SelectedRow;

            txtId.Text = row.Cells[0].Text;
            txtNombre.Text = row.Cells[1].Text;
            txtApellido.Text = row.Cells[2].Text;
            txtDNI.Text = row.Cells[3].Text;
            txtEmail.Text = row.Cells[4].Text;
            txtTelefono.Text = row.Cells[5].Text;

            ClienteGestionDTO cli = negocio.ObtenerPorId(int.Parse(txtId.Text));

            txtIdUsuario.Text = cli.IdUsuario.ToString();
            txtDireccion.Text = cli.Direccion;
            txtCP.Text = cli.CP;
        }

        private void ValidarFormulario()
        {
            // Campos obligatorios
            Validaciones.Requerido(txtNombre.Text, "Nombre Cliente");
            Validaciones.Requerido(txtApellido.Text, "Apellido Cliente");
            Validaciones.Requerido(txtDNI.Text, "DNI");
            Validaciones.Requerido(txtEmail.Text, "Email");
            Validaciones.Requerido(txtIdUsuario.Text, "Usuario");
            Validaciones.Requerido(txtTelefono.Text, "Telefono");
            Validaciones.Requerido(txtDireccion.Text, "Dirección");
            Validaciones.Requerido(txtCP.Text, "CP");

        }

        protected void btnIrGestionUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionUsuario.aspx");
        }

        protected void btnLimpiar1_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            CargarGrilla();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            GrillaBusqueda(txtFiltro.Text);
        }

        private void GrillaBusqueda(string filtro = "")
        {
            var lista = negocio.Listar();

            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                lista = lista.Where(p =>
                    p.Nombre.ToLower().Contains(filtro) ||
                    p.Apellido.ToLower().Contains(filtro) ||
                    p.DNI.ToString().Contains(filtro) ||
                    p.Email.ToLower().Contains(filtro) ||
                    p.Telefono.ToLower().Contains(filtro)
                ).ToList();
            }

            var vista = lista.Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Apellido,
                p.DNI,
                p.Email,
                p.Telefono
            }).ToList();

            dgvClientes.DataSource = vista;
            dgvClientes.DataBind();

        }
    }
}