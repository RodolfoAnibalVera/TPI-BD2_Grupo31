<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ofertas.aspx.cs" Inherits="E_Commerce_Bookstore.Ofertas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Ofertas</h1>
    <asp:UpdatePanel ID="updOfertas" runat="server">
    <ContentTemplate>
        <div class="row">
            <asp:Repeater ID="repOfertas" runat="server" OnItemDataBound="repOfertas_ItemDataBound">
                <ItemTemplate>
                    <div class="col-md-4 mb-3">
                        <div class="card h-100 position-relative">
                            <asp:Literal ID="litOferta" runat="server" />
                            <asp:Literal ID="litBestSeller" runat="server" />
                            <img class="card-img-top" src='<%# Eval("ImagenUrl") %>' alt="Portada">
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title"><%# Eval("Titulo") %></h5>
                                <p class="fw-bold text-danger">Precio Oferta: $ <%# Eval("PrecioVenta", "{0:N2}") %></p>
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
