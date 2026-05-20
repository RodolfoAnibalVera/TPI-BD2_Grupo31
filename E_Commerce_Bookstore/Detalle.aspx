<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="E_Commerce_Bookstore.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="card shadow-lg p-4" style="background-color: #c6c7c8a6;">
            <div class="row">
                <!-- Imagen del libro -->
                <div class="col-md-4 position-relative">
                    <asp:Literal ID="litBestSeller" runat="server" />
                    <asp:Image ID="imgLibro" runat="server" CssClass="img-fluid rounded" ImageUrl="https://via.placeholder.com/300x400?text=Sin+imagen" />
                </div>

                <!-- Detalles del libro -->
                <div class="col-md-8">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="Sin datos"></asp:Label></h2>
                    <p class="text-muted">
                        Autor:
               
                        <asp:Label ID="lblAutor" runat="server" Text="Sin datos"></asp:Label>
                    </p>
                    <p class="text-muted">
                        Editorial:
               
                        <asp:Label ID="lblEditorial" runat="server" Text="Sin datos"></asp:Label>
                    </p>
                    <p class="text-muted">
                        Categoría:
                        <asp:Label ID="lblCategoria" runat="server" Text="Sin datos"></asp:Label>
                    </p>
                    <p>
                        <asp:Label ID="lblDescripcion" runat="server" Text="Sin datos"></asp:Label>
                    </p>
                    <p class="text-muted">
                        Idioma:
                        <asp:Label ID="lblIdioma" runat="server" Text="Sin datos"></asp:Label>
                    </p>

                    <p class="text-muted">
                        Año de edición:
                        <asp:Label ID="lblAnioEdicion" runat="server" Text="Sin datos"></asp:Label>
                    </p>

                    <p class="text-muted">
                        Paginas:
                        <asp:Label ID="lblPagina" runat="server" Text="Sin datos"></asp:Label>
                    </p>

                    <h4 class="text-success">Precio: $<asp:Label ID="lblPrecio" runat="server" Text="Sin datos"></asp:Label></h4>
                    <p id="pStock" runat="server" class="text-success">
                        Stock disponible:
                        <asp:Label ID="lblStock" runat="server" Text="Sin datos"></asp:Label>
                        <span id="badgeStock" runat="server" class="badge bg-warning text-dark ms-2" visible="false">¡Última unidad!</span>
                    </p>


                    <!-- Incrementar cantidad + agregar + volver-->
                    <asp:UpdatePanel ID="updDetalleCarrito" runat="server">
                        <ContentTemplate>
                            <div class="d-flex align-items-center gap-2 mb-3 flex-wrap">
                                <!-- Selector de cantidad -->
                                <div class="input-group" runat="server" id="grupoCantidad" style="max-width: 150px;">
                                    <button type="button" class="btn btn-outline-light border-2" style="background-color: gray;" onclick="decrementCantidad()">−</button>
                                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control text-center" Text="1" onkeydown="return false;" />
                                    <button type="button" class="btn btn-outline-light" style="background-color: gray;" onclick="incrementCantidad()">+</button>
                                </div>

                                <!-- Botón agregar al carrito -->
                                <asp:Button ID="btnAgregarCarrito" runat="server" CssClass="btn btn-primary border-2" Text="Agregar al carrito" OnClick="btnAgregarCarrito_Click" />
                                <!-- Botón volver -->
                                <asp:Button ID="btnVolver" runat="server" CssClass="btn btn-success border-2" Text="Volver" OnClick="btnVolver_Click"  CausesValidation="false"/>
                                <asp:Label ID="lblMensajeAgregado" runat="server" CssClass="text-success mt-2 d-block" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
            </div>
        </div>

    </div>
    <!-- Sección de libros sugeridos -->
    <asp:Panel ID="pnlSugerencias" runat="server">
        <div class="container text-center mt-5">
            <h3 class="text-dark mb-4">Libros sugeridos</h3>
            <div class="row justify-content-center g-3">
                <asp:Repeater ID="repSugeridos" runat="server" OnItemDataBound="repSugeridos_ItemDataBound">
                    <ItemTemplate>
                        <div class="col-6 col-md-3 text-center">
                            <div class="position-relative d-inline-block">
                                <asp:Literal ID="litBestSeller" runat="server" />
                                <asp:HyperLink runat="server" NavigateUrl='<%# "Detalle.aspx?id=" + Eval("Id") %>'>
                                          <asp:Image runat="server" CssClass="img-fluid rounded w-75"
                                                     ImageUrl='<%# Eval("ImagenUrl") %>' AlternateText='<%# Eval("Titulo") %>' />
                                </asp:HyperLink>
                            </div>
                            <asp:Label runat="server" CssClass="text-dark mt-2 d-block" Text='<%# Eval("Titulo") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>


    <!-- Para incrementar la cantidad de libros para mandar al carrito -->
    <script>
        // Valor de stock disponible desde el backend
        var stockDisponible = parseInt('<%= lblStock.Text %>') || 0;

        function incrementCantidad() {
            var txt = document.getElementById('<%= txtCantidad.ClientID %>');
            var valor = parseInt(txt.value) || 0;

            if (valor < stockDisponible) {
                txt.value = valor + 1;
            }
        }

        function decrementCantidad() {
            var txt = document.getElementById('<%= txtCantidad.ClientID %>');
            var valor = parseInt(txt.value) || 0;

            if (valor > 1) {
                txt.value = valor - 1;
            }
        }
    </script>
</asp:Content>
