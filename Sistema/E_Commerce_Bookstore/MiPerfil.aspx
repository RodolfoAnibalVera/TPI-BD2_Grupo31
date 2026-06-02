<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="E_Commerce_Bookstore.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container d-flex justify-content-center mt-5">
        <div class="card shadow w-100" style="max-width: 800px;">
            <div class="card-body">

                <h2 class="text-center mb-4">Mi Perfil</h2>
                <asp:Label ID="lblSaludo" runat="server" CssClass="h5 text-start d-block mb-4" />

                <div class="row">
                    <div class="col-md-8">
                        <div class="row mb-2">
                            <label class="col-sm-4 col-form-label text-dark fw-semibold">Nombre:</label>
                            <div class="col-sm-8">
                                <asp:Label ID="lblNombre" runat="server" CssClass="text-secondary" />
                            </div>
                        </div>
                        <div class="row mb-2">
                            <label class="col-sm-4 col-form-label text-dark fw-semibold">Apellido:</label>
                            <div class="col-sm-8">
                                <asp:Label ID="lblApellido" runat="server" CssClass="text-secondary" />
                            </div>
                        </div>
                        <div class="row mb-2">
                            <label class="col-sm-4 col-form-label text-dark fw-semibold">DNI:</label>
                            <div class="col-sm-8">
                                <asp:Label ID="lblDni" runat="server" CssClass="text-secondary" />
                            </div>
                        </div>
                        <div class="row mb-2">
                            <label class="col-sm-4 col-form-label text-dark fw-semibold">Email:</label>
                            <div class="col-sm-8">
                                <asp:Label ID="lblEmail" runat="server" CssClass="text-secondary" />
                            </div>
                        </div>
                        <div class="row mb-2">
                            <label class="col-sm-4 col-form-label text-dark fw-semibold">Teléfono:</label>
                            <div class="col-sm-8">
                                <asp:Label ID="lblTelefono" runat="server" CssClass="text-secondary" />
                            </div>
                        </div>
                        <div class="row mb-2">
                            <label class="col-sm-4 col-form-label text-dark fw-semibold">Dirección:</label>
                            <div class="col-sm-8">
                                <asp:Label ID="lblDireccion" runat="server" CssClass="text-secondary" />
                            </div>
                        </div>
                        <div class="row mb-2">
                            <label class="col-sm-4 col-form-label text-dark fw-semibold">Código Postal:</label>
                            <div class="col-sm-8">
                                <asp:Label ID="lblCP" runat="server" CssClass="text-secondary" />
                            </div>
                        </div>
                        <asp:HyperLink ID="lnkEditar" runat="server" NavigateUrl="ModificacionCliente.aspx" CssClass="btn btn-primary mt-3">
                            Editar
                        </asp:HyperLink>

                    </div>
                    <div class="col-md-4 d-flex flex-column justify-content-start ps-md-4">
                        <asp:HyperLink ID="lnkCarrito" runat="server" NavigateUrl="Carrito.aspx" CssClass="btn btn-outline-dark mb-2">
                            Ver carrito
                        </asp:HyperLink>
                        <asp:HyperLink ID="lnkCatalogo" runat="server" NavigateUrl="Catalogo.aspx" CssClass="btn btn-outline-dark mb-2">
                            Seguir comprando
                        </asp:HyperLink>
                        <asp:HyperLink ID="lnkPedidos" runat="server"
                            NavigateUrl="MisPedidos.aspx?origen=perfil"
                            CssClass="btn btn-outline-dark">
                            Mis pedidos
                        </asp:HyperLink>
                        <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar sesión" CssClass="btn btn-danger mt-3" OnClick="btnCerrarSesion_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
