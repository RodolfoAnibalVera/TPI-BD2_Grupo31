<%@ Page Title="Catálogo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="E_Commerce_Bookstore.Catalogo" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mb-4">Catálogo</h2>

    <!-- BUSCADOR (Autor / ISBN / Categoría) -->
    <div class="mb-4">
        <div class="input-group">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                placeholder="Buscar por autor, ISBN o Titulo..." />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
        </div>
        <small class="text-muted">Ej.: “Gaiman”, “978…”, “Fantasía”</small>
    </div>

    <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger d-block mb-3"></asp:Label>

    <asp:UpdatePanel ID="updCatalogo" runat="server">
        <ContentTemplate>
            <div class="row">
                <asp:Repeater ID="repLibros" runat="server" OnItemCommand="repLibros_ItemCommand" OnItemDataBound="repLibros_ItemDataBound">
                    <ItemTemplate>
                        <div class="col-md-4 mb-3">
                            <div class="card h-100 position-relative">
                                <asp:Literal ID="litBestSeller" runat="server" />
                                <img class="card-img-top" src='<%# Eval("ImagenUrl") %>' alt="Portada">
                                <div class="card-body d-flex flex-column">
                                    <h5 class="card-title"><%# Eval("Titulo") %></h5>
                                    <p class="card-text"><%# Eval("Descripcion") %></p>
                                    <p class="fw-bold">Precio: $ <%# Eval("PrecioVenta", "{0:N2}") %></p>

                                    <div class="mt-auto d-flex gap-2">
                                        <asp:Button ID="btnDetalle" runat="server"
                                            Text="Ver Detalle" CommandName="Detalle"
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary" OnCommand="btnAccionCommand" />
                                        <asp:Button ID="btnComprar" runat="server"
                                            Text="Comprar" CommandName="Comprar"
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-success" OnCommand="btnAccionCommand" />
                                    </div>
                                    <asp:Label ID="lblLocalMensaje" runat="server" CssClass="text-success small mt-2" />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
