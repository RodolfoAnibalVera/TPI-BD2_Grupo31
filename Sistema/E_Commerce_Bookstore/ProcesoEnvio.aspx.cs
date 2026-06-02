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
    public partial class ProcesoEnvio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si el usuario no esta logueado, lo mandamos al login
            if (Session["IdCliente"] == null)
            {
                Response.Redirect("MiCuenta.aspx?ReturnUrl=ProcesoEnvio.aspx", false);
                return;
            }

            try
            {
                if (!IsPostBack)
                {
                    bool envioADomicilio = false;
                    if (Session["EnvioADomicilio"] != null)
                    {
                        bool.TryParse(Session["EnvioADomicilio"].ToString(), out envioADomicilio);
                    }

                    int idCliente = Convert.ToInt32(Session["IdCliente"]);

                    // Mostrar/ocultar paneles segun el metodo
                    pnlEntrega.Visible = envioADomicilio;

                    if (envioADomicilio)
                    {
                        chkFacturacion.Visible = true;
                        pnlFacturacion.Visible = chkFacturacion.Checked;
                        ToggleValidators(pnlEntrega, true);
                        ToggleValidators(pnlFacturacion, pnlFacturacion.Visible);
                        
                        PrecargarDatosDesdeEnvios(idCliente);

                        PrecargarEmailDesdeClientes(idCliente);

                    }
                    else
                    {
                        chkFacturacion.Visible = false;
                        pnlFacturacion.Visible = true;
                        ToggleValidators(pnlEntrega, false);
                        ToggleValidators(pnlFacturacion, true);

                        PrecargarDatosFacturacionDesdeClientes(idCliente);
                    }
                }
                                                                            

                // Ocultar navbar
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

        private void PrecargarDatosDesdeEnvios(int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT TOP 1 
                        e.NombreEnvio,
                        e.ApellidoEnvio,
                        e.DireccionEnvio,
                        e.Barrio,
                        e.Ciudad,
                        e.Departamento,
                        e.CPEnvio
                    FROM ENVIOS e
                    INNER JOIN PEDIDOS p ON e.IdPedido = p.Id
                    WHERE p.IdCliente = @IdCliente
                    ORDER BY e.Fecha DESC
                ");

                datos.setearParametro("@IdCliente", idCliente);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    if (string.IsNullOrEmpty(txtNombre.Text))
                        txtNombre.Text = datos.Lector["NombreEnvio"].ToString();

                    if (string.IsNullOrEmpty(txtApellido.Text))
                        txtApellido.Text = datos.Lector["ApellidoEnvio"].ToString();

                    if (string.IsNullOrEmpty(txtCalle.Text))
                        txtCalle.Text = datos.Lector["DireccionEnvio"].ToString();

                    if (string.IsNullOrEmpty(txtBarrio.Text))
                        txtBarrio.Text = datos.Lector["Barrio"].ToString();

                    if (string.IsNullOrEmpty(txtCiudad.Text))
                        txtCiudad.Text = datos.Lector["Ciudad"].ToString();

                    if (string.IsNullOrEmpty(txtDepto.Text))
                        txtDepto.Text = datos.Lector["Departamento"].ToString();

                    if (string.IsNullOrEmpty(txtCP.Text))
                        txtCP.Text = datos.Lector["CPEnvio"].ToString();
                }
            }
            catch
            {
                
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private void PrecargarDatosFacturacionDesdeClientes(int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT Nombre, Apellido, Email, Direccion, CP
                    FROM CLIENTES
                    WHERE Id = @IdCliente
                ");
                datos.setearParametro("@IdCliente", idCliente);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    // Email de contacto
                    if (string.IsNullOrEmpty(txtEmail.Text) && datos.Lector["Email"] != DBNull.Value)
                        txtEmail.Text = datos.Lector["Email"].ToString();

                    // Facturacion
                    if (string.IsNullOrEmpty(txtFacNombre.Text) && datos.Lector["Nombre"] != DBNull.Value)
                        txtFacNombre.Text = datos.Lector["Nombre"].ToString();

                    if (string.IsNullOrEmpty(txtFacApellido.Text) && datos.Lector["Apellido"] != DBNull.Value)
                        txtFacApellido.Text = datos.Lector["Apellido"].ToString();

                    if (string.IsNullOrEmpty(txtFacCalle.Text) && datos.Lector["Direccion"] != DBNull.Value)
                        txtFacCalle.Text = datos.Lector["Direccion"].ToString();

                    if (string.IsNullOrEmpty(txtFacCP.Text) && datos.Lector["CP"] != DBNull.Value)
                        txtFacCP.Text = datos.Lector["CP"].ToString();
                }
            }
            catch
            {
                
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        
        private void PrecargarEmailDesdeClientes(int idCliente)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text))
                return;

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT Email
                    FROM CLIENTES
                    WHERE Id = @IdCliente
                ");
                datos.setearParametro("@IdCliente", idCliente);
                datos.ejecutarLectura();

                if (datos.Lector.Read() && datos.Lector["Email"] != DBNull.Value)
                {
                    txtEmail.Text = datos.Lector["Email"].ToString();
                }
            }
            catch
            {
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        // habilitar/deshabilitar validadores dentro de un contenedor
        private void ToggleValidators(Control root, bool enabled)
        {
            if (root == null) return;

            foreach (Control c in root.Controls)
            {
                var val = c as BaseValidator;
                if (val != null) val.Enabled = enabled;

                if (c.HasControls())
                    ToggleValidators(c, enabled);
            }
        }
        protected void chkFacturacion_CheckedChanged(object sender, EventArgs e)
        {
            pnlFacturacion.Visible=chkFacturacion.Checked;

            ToggleValidators(pnlFacturacion, pnlFacturacion.Visible);
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // Dato de contacto
                Session["EmailContacto"] = txtEmail.Text.Trim();

                // Envio
                Session["NombreEnvio"] = txtNombre.Text.Trim();
                Session["ApellidoEnvio"] = txtApellido.Text.Trim();
                Session["DireccionEnvio"] = txtCalle.Text.Trim();
                Session["BarrioEnvio"] = txtBarrio.Text.Trim();
                Session["CiudadEnvio"] = txtCiudad.Text.Trim();
                Session["DeptoEnvio"] = txtDepto.Text.Trim();
                Session["CPEnvio"] = txtCP.Text.Trim();

                // Facturacion
                Session["NombreFacturacion"] = txtFacNombre.Text.Trim();
                Session["ApellidoFacturacion"] = txtFacApellido.Text.Trim();
                Session["DireccionFacturacion"] = txtFacCalle.Text.Trim();
                Session["BarrioFacturacion"] = txtFacBarrio.Text.Trim();
                Session["CiudadFacturacion"] = txtFacCiudad.Text.Trim();
                Session["DeptoFacturacion"] = txtFacDepto.Text.Trim();
                Session["CPFacturacion"] = txtFacCP.Text.Trim();

                Response.Redirect("ProcesoPago.aspx", false);
            }
            catch
            {
                Response.Redirect("Error.aspx", false);
            }
        }

    }
}