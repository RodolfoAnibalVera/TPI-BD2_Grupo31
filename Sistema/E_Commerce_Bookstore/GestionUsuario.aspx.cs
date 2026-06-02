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
    public partial class GestionUsuario : System.Web.UI.Page
    {
        UsuarioNegocio negocio = new UsuarioNegocio();
        TipoUsuarioNegocio negocioTipo = new TipoUsuarioNegocio();
        ValidacionGestion validar = new ValidacionGestion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
                CargarTipos();
            }
        }

        private void CargarGrilla()
        {
            var lista = negocio.ListarConTipo();
            dgvUsuarios.DataSource = lista;
            dgvUsuarios.DataBind();
        }
        private void CargarTipos()
        {
            ddlTipoUsuario.DataSource = negocioTipo.Listar();
            ddlTipoUsuario.DataTextField = "Rol";
            ddlTipoUsuario.DataValueField = "Id";
            ddlTipoUsuario.DataBind();

            ddlTipoUsuario.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione...", "0"));
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtNombreUsuario.Text = "";
            txtContrasena.Text = "";
            txtEmail.Text = "";
            ddlTipoUsuario.SelectedIndex = 0;
            chkActivo.Checked = false;
            lblMensaje.Text = "";
            txtFiltro.Text = "";
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtId.Text);
                negocio.Eliminar(id);
                lblMensaje.Text = "<div class='alert alert-danger'> Usuario eliminado </div>";
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Error: No fue posible eliminar usuario" + ex.Message + "</div>";
            }

        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                
                Usuario u = ObtenerDesdeFormulario();
                u.Id = int.Parse(txtId.Text);

                if (validar.EmailExiste(u.Email, u.Id))
                {
                    lblMensaje.Text = "❌ Este email ya pertenece a otro usuario.";
                    return;
                }

                negocio.Modificar(u);
                lblMensaje.Text = "<div class='alert alert-success'> Usuario modificado correctamente. </div>";
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Error: No fue posible modificar usuario." + ex.Message + "</div>";
            }

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                Usuario u = ObtenerDesdeFormulario();

                if (validar.EmailExiste(u.Email))
                    throw new Exception($" ❌ El email ya está registrado.");

                negocio.Agregar(u);
                lblMensaje.Text = "<div class='alert alert-success'> Usuario agregado correctamente. </div>";
                CargarGrilla();

            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Error: No fue posible agregar usuario." + ex.Message + "</div>";
            }
        }

        private Usuario ObtenerDesdeFormulario()
        {
            ValidarFormulario();
            return new Usuario
            {
                NombreUsuario = txtNombreUsuario.Text,
                Contrasena = txtContrasena.Text,
                Email = txtEmail.Text,
                IdTipoUsuario = new TipoUsuario { Id = int.Parse(ddlTipoUsuario.SelectedValue) },
                Activo = chkActivo.Checked
            };
        }

        protected void dgvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fila = dgvUsuarios.SelectedRow;

            txtId.Text = fila.Cells[0].Text;
            txtNombreUsuario.Text = fila.Cells[1].Text;
            txtContrasena.Text = fila.Cells[2].Text;        // Contraseña (oculta)
            txtEmail.Text = fila.Cells[3].Text;

            ddlTipoUsuario.SelectedValue = negocio.ObtenerIdTipo(int.Parse(txtId.Text)).ToString();

            chkActivo.Checked = ((CheckBox)fila.Cells[5].Controls[0]).Checked;

            
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session["EmailCliente"] = txtEmail.Text;
            Session["IdUsuarioCliente"] = txtId.Text;

            Response.Redirect("GestionClientes.aspx");
        }

        protected void btnLimpiar1_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            cargarGrilla();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarGrilla(txtFiltro.Text);
        }

        private void cargarGrilla(string filtro = "")
        {
            var lista = negocio.ListarConTipo();

            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                lista = lista.Where(p =>
                    p.NombreUsuario.ToLower().Contains(filtro) ||
                    p.Email.ToLower().Contains(filtro) ||
                    p.Rol.ToLower().Contains(filtro)
                ).ToList();
            }

            var vista = lista.Select(p => new
            {
                p.Id,
                p.NombreUsuario,
                p.Email,
                p.Rol,
                p.Activo
            }).ToList();

            dgvUsuarios.DataSource = vista;
            dgvUsuarios.DataBind();

        }

        private void ValidarFormulario()
        {
            // Validar Email
            if (!validar.EsEmailValido(txtEmail.Text))
                throw new Exception($" El campo Email no tiene el formato correcto.");
            
            // Campos obligatorios
            Validaciones.Requerido(txtNombreUsuario.Text, "Nombre Usuario");
            Validaciones.Requerido(txtEmail.Text, "Email");
            Validaciones.Requerido(txtContrasena.Text, "Contraseña");

            //Validar Tipo de usuario
            if (string.IsNullOrEmpty(ddlTipoUsuario.SelectedValue) || ddlTipoUsuario.SelectedValue == "0")
            throw new Exception(" ⚠ Debe seleccionar un tipo de usuario.");
             
        }
    }
}