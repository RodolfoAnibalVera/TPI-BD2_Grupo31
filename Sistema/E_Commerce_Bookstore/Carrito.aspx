<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="E_Commerce_Bookstore.Carrito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <h2 class="mb-3 text-center">🛒 Carrito de Compras</h2>

                <asp:UpdatePanel ID="updCarrito" runat="server">
                    <ContentTemplate>

                        <asp:Repeater ID="rptCarrito" runat="server" OnItemCommand="rptCarrito_ItemCommand">
                            <ItemTemplate>
                                <div class="row align-items-center mb-3 border-bottom pb-2">
                                    <div class="col-2">
                                        <asp:Image ID="imgProducto" runat="server" ImageUrl='<%# Eval("Libro.ImagenUrl") %>' CssClass="img-fluid rounded" AlternateText="Imagen" />
                                    </div>
                                    <div class="col-4">
                                        <strong><%# Eval("Libro.Titulo") %></strong><br />
                                        <small>Precio: $<%# Eval("PrecioUnitario", "{0:N2}") %></small>
                                    </div>
                                    <div class="col-3 d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" CommandName="Decrementar" CommandArgument='<%# Eval("IdLibro") %>' CssClass="btn btn-sm btn-outline-secondary me-2" Text="−" />
                                        <span class="mx-2"><%# Eval("Cantidad") %></span>
                                        <asp:Button runat="server" CommandName="Incrementar" CommandArgument='<%# Eval("IdLibro") %>' CssClass="btn btn-sm btn-outline-success ms-2" Text="+" />
                                    </div>
                                    <div class="col-2 text-end">
                                        $<%# Eval("Subtotal", "{0:N2}") %>
                                    </div>
                                    <div class="col-1 text-end">
                                        <asp:Button runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("IdLibro") %>' CssClass="btn btn-sm btn-outline-danger" Text="🗑️" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="form-check mt-3">
                            <asp:CheckBox ID="chkEnvioDomicilio" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="chkEnvioDomicilio">
                                <i class="fas fa-truck me-2"></i>Quiero envío a domicilio
                            </label>
                        </div>

                        <div class="row mt-4">
                            <div class="col-12 text-end">
                                <h5>Total: $<asp:Label ID="lblTotal" runat="server" CssClass="fw-bold" /></h5>
                            </div>
                        </div>
                        <asp:Label ID="lblError" runat="server" CssClass="d-none" />
                        <div class="row mt-3">
                            <div class="col-6">
                                <asp:Button ID="btnVolver" runat="server" Text="← Volver" CssClass="btn btn-secondary w-100" OnClick="btnVolver_Click" />
                            </div>
                            <div class="col-6">
                                <asp:Button ID="btnComprar" runat="server" Text="Comprar" CssClass="btn btn-primary w-100" OnClick="btnComprar_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>
