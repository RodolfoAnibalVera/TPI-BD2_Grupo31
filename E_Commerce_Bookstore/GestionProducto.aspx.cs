using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class GestionProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdTipoUsuario"] == null || Session["IdTipoUsuario"].ToString() != "1")
            {
                Session["error"] = new Exception("Acceso denegado: solo administradores pueden ingresar a esta página.");
                Response.Redirect("Error.aspx");
            }
            if (!IsPostBack)
            {
                listarActivos();
                cargarCategorias();
                cargarEditorial();
                cargarAutor();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                Libro nuevo = new Libro
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    ISBN = txtISBN.Text.Trim(),
                    Idioma = txtIdioma.Text.Trim(),
                    AnioEdicion = int.Parse(txtAnioEdicion.Text),
                    Paginas = int.Parse(txtPaginas.Text),
                    Stock = int.Parse(txtStock.Text),
                    Activo = chkActivo.Checked,
                    PrecioCompra = decimal.Parse(txtPrecioCompra.Text),
                    PrecioVenta = decimal.Parse(txtPrecioVenta.Text),
                    PorcentajeGanancia = decimal.Parse(txtPorcentajeGanancia.Text),
                    ImagenUrl = txtImagenUrl.Text.Trim(),

                    Editorial = new Editorial { Id = int.Parse(ddlEditorial.SelectedValue) },
                    Autor = new Autor { Id = int.Parse(ddlAutor.SelectedValue) },

                    Categoria = new Dominio.Categoria
                    {
                        Id = int.Parse(ddlCategoria.SelectedValue)
                    }
                };

                ValidacionGestion validar = new ValidacionGestion();
                var errores = validar.ValidarLibro(nuevo);

                if (errores.Count > 0)
                {
                    // Mostrar todos los errores juntos
                    lbMensaje.Text = "⚠️ Se encontraron los siguientes errores:<br/>" + string.Join("<br/>", errores);
                    lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                    return;
                }

                LibroNegocio negocio = new LibroNegocio();
                negocio.AgregarLibro(nuevo);

                lbMensaje.Text = "✅ Libro agregado correctamente.";
                lbMensaje.ForeColor = System.Drawing.Color.Green;

                CargarGrilla();
                LimpiarCampos();
            }
            catch (FormatException)
            {
                lbMensaje.Text = "⚠️ Verifica los valores numéricos (Año, Páginas, Precios, Stock y Categoria, etc.).";
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error al guardar el libro: " + ex.Message;
                lbMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void dgvArticulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbMensaje.Text = "";

            var seleccion = dgvArticulo.SelectedRow.Cells[0].Text;
            var id = dgvArticulo.SelectedDataKey.Value.ToString();

            LibroNegocio negocio = new LibroNegocio();

            if (!string.IsNullOrEmpty(id))
            {
                int IdLib = int.Parse(id);
                var listaTemporal = negocio.listarGrilla();
                Libro lib = listaTemporal.Find(x => x.Id == IdLib);

                if (lib != null)
                {

                    ddlCategoria.SelectedValue = lib.Categoria.Id.ToString();
                    txtId.Text = lib.Id.ToString();
                    txtTitulo.Text = lib.Titulo;
                    ddlAutor.SelectedValue = lib.Autor.Id.ToString();
                    txtDescripcion.Text = lib.Descripcion;
                    ddlEditorial.SelectedValue = lib.Editorial.Id.ToString();
                    txtIdioma.Text = lib.Idioma;
                    txtImagenUrl.Text = lib.ImagenUrl;
                    txtISBN.Text = lib.ISBN;
                    txtPaginas.Text = lib.Paginas.ToString();
                    txtPorcentajeGanancia.Text = lib.PorcentajeGanancia.ToString();
                    txtPrecioCompra.Text = lib.PrecioCompra.ToString();
                    txtPrecioVenta.Text = lib.PrecioVenta.ToString();
                    txtAnioEdicion.Text = lib.AnioEdicion.ToString();
                    chkActivo.Checked = lib.Activo;
                    txtStock.Text = lib.Stock.ToString();
                    ddlCategoria.SelectedValue = lib.Categoria.Id.ToString();
                    imgPortada.ImageUrl = lib.ImagenUrl;

                }

            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            lbMensaje.Text = "";
        }

        private void CargarGrilla()
        {
            LibroNegocio negocio = new LibroNegocio();
            dgvArticulo.DataSource = negocio.listarGrilla();
            dgvArticulo.DataBind();
        }

        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtTitulo.Text = "";
            txtDescripcion.Text = "";
            txtISBN.Text = "";
            txtIdioma.Text = "";
            txtAnioEdicion.Text = "";
            txtPaginas.Text = "";
            txtStock.Text = "";
            chkActivo.Checked = true;
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtPorcentajeGanancia.Text = "";
            txtImagenUrl.Text = "";
            ddlEditorial.ClearSelection();
            ddlAutor.ClearSelection();
            ddlCategoria.ClearSelection();
            imgPortada.ImageUrl = "";
            listarActivos();
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            LibroNegocio negocio = new LibroNegocio();
            try
            {
                Libro libro = new Libro
                {
                    Id = int.Parse(txtId.Text),
                    Titulo = txtTitulo.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    ISBN = txtISBN.Text.Trim(),
                    Idioma = txtIdioma.Text.Trim(),
                    AnioEdicion = int.Parse(txtAnioEdicion.Text),
                    Paginas = int.Parse(txtPaginas.Text),
                    Stock = int.Parse(txtStock.Text),
                    Activo = chkActivo.Checked,
                    PrecioCompra = decimal.Parse(txtPrecioCompra.Text),
                    PrecioVenta = decimal.Parse(txtPrecioVenta.Text),
                    PorcentajeGanancia = decimal.Parse(txtPorcentajeGanancia.Text),
                    ImagenUrl = txtImagenUrl.Text.Trim(),

                    Editorial = new Editorial { Id = int.Parse(ddlEditorial.SelectedValue) },
                    Autor = new Autor { Id = int.Parse(ddlAutor.SelectedValue) },

                    Categoria = new Dominio.Categoria
                    {
                        Id = int.Parse(ddlCategoria.SelectedValue)
                    }
                };


                ValidacionGestion validar = new ValidacionGestion();
                var errores = validar.ValidarLibro(libro);

                if (errores.Count > 0)
                {
                    // Mostrar todos los errores juntos
                    lbMensaje.Text = "⚠️ Se encontraron los siguientes errores:<br/>" + string.Join("<br/>", errores);
                    lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                    return;
                }

                negocio.ModificarLibro(libro);

                lbMensaje.Text = "✅ Libro modificado correctamente.";
                lbMensaje.ForeColor = System.Drawing.Color.Green;

                CargarGrilla();
                LimpiarCampos();
            }
            catch (FormatException)
            {
                lbMensaje.Text = "⚠️ Verifica los valores numéricos (Año, Páginas, Precios, etc.).";
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error al modificar el libro: " + ex.Message;
                lbMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtImagenUrl.Text))
                {
                    imgPortada.ImageUrl = txtImagenUrl.Text;
                }
                else
                {
                    imgPortada.ImageUrl = "https://img.freepik.com/free-photo/image-icon-front-side-white-background_187299-40166.jpg?semt=ais_hybrid&w=740&q=80";
                }
            }
            catch
            {
                imgPortada.ImageUrl = "https://img.freepik.com/free-photo/image-icon-front-side-white-background_187299-40166.jpg?semt=ais_hybrid&w=740&q=80";
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedValue;
            string criterio = ddlCriterio.SelectedValue;
            string filtro = txtFiltro.Text.Trim();
            string estado = ddlEstado.SelectedValue;

            LibroNegocio negocio = new LibroNegocio();
            try
            {
                var listaFiltrada = negocio.Filtrar(campo, criterio, filtro, estado);
                dgvArticulo.DataSource = listaFiltrada;
                dgvArticulo.DataBind();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error al filtrar: " + ex.Message;
                lbMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCriterio.Items.Clear();
            ddlCriterio.Items.Add("Seleccione...");

            if (ddlCampo.SelectedValue == "PrecioVenta" || ddlCampo.SelectedValue == "Stock")
            {
                ddlCriterio.Items.Add("Mayor que");
                ddlCriterio.Items.Add("Menor que");
                ddlCriterio.Items.Add("Igual a");
            }
            else
            {
                ddlCriterio.Items.Add("Contiene");
                ddlCriterio.Items.Add("Empieza con");
                ddlCriterio.Items.Add("Termina con");
                ddlCriterio.Items.Add("Igual a");
            }
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            lbMensaje.Text = "";
        }

        protected void btnMostrarConfirmacion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                lbMensaje.Text = "⚠️ Seleccione un libro antes de eliminar del catalogo.";
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }
            else
            {
                pnlConfirmacion.Visible = true;
                lbMensaje.Text = "";
                UpdatePanel1.Update();
            }
        }

        protected void btnCancelarEliminar_Click(object sender, EventArgs e)
        {
            pnlConfirmacion.Visible = false;
            lbMensaje.Text = "❎ Eliminación del catalogo cancelada.";
            lbMensaje.ForeColor = System.Drawing.Color.Gray;
            UpdatePanel1.Update();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    lbMensaje.Text = "⚠️ Seleccione un libro antes de eliminar del catalogo.";
                    lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                    return;
                }

                int id = int.Parse(txtId.Text);
                LibroNegocio negocio = new LibroNegocio();
                negocio.Desactivar(id);

                lbMensaje.Text = "✅ Libro eliminado del catalogo correctamente.";
                lbMensaje.ForeColor = System.Drawing.Color.Green;

                pnlConfirmacion.Visible = false; // ocultamos el panel
                LimpiarCampos();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error al eliminar del catalogo: " + ex.Message;
                lbMensaje.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                UpdatePanel1.Update(); // refresca la UI sin recargar la página
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                lbMensaje.Text = "Ingrese un texto para buscar.";
                lbMensaje.ForeColor = System.Drawing.Color.Red;
                return;
            }

            LibroNegocio negocio = new LibroNegocio();

            try
            {
                List<Libro> resultados = negocio.Buscar(texto);

                if (resultados != null && resultados.Count > 0)
                {
                    dgvArticulo.DataSource = resultados;
                    dgvArticulo.DataBind();
                    lbMensaje.Text = $"{resultados.Count} resultados encontrados.";
                    lbMensaje.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    dgvArticulo.DataSource = null;
                    dgvArticulo.DataBind();
                    lbMensaje.Text = "No se encontraron resultados.";
                    lbMensaje.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "Error al realizar la búsqueda.";
                lbMensaje.ForeColor = System.Drawing.Color.Red;

            }
        }

        private void cargarCategorias()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            ddlCategoria.DataSource = negocio.Listar();
            ddlCategoria.DataTextField = "Nombre";
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataBind();

            ddlCategoria.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void cargarEditorial()
        {
            EditorialNegocio negocioEditorial = new EditorialNegocio();
            ddlEditorial.DataSource = negocioEditorial.Listar();
            ddlEditorial.DataTextField = "Nombre";
            ddlEditorial.DataValueField = "Id";
            ddlEditorial.DataBind();

            ddlEditorial.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void cargarAutor()
        {
            AutorNegocio negocioAutor = new AutorNegocio();
            ddlAutor.DataSource = negocioAutor.Listar();
            ddlAutor.DataTextField = "Nombre";
            ddlAutor.DataValueField = "Id";
            ddlAutor.DataBind();

            ddlAutor.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }
        private void listarActivos()
        {
            LibroNegocio negocio = new LibroNegocio();

            List<Libro> lista = negocio.listarGrilla();

            List<Libro> listarActivos = lista.FindAll(x => x.Activo == true);

            Session["listaArticulos"] = listarActivos;
            dgvArticulo.DataSource = listarActivos;
            dgvArticulo.DataBind();

            dgvArticulo.CssClass = "table table-striped table-info";
        }

        protected void btnGestionAutor_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionAutor.aspx", false);
        }

        protected void btnNuevaCat_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionCategoria.aspx", false);
        }

        protected void btnNuevaEdi_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionEditorial.aspx", false);
        }
    }
}