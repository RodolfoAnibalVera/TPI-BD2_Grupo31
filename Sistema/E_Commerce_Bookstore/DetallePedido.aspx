<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetallePedido.aspx.cs" Inherits="E_Commerce_Bookstore.DetallePedido" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4">

        <!-- Título -->
        <h2 class="text-primary fw-bold mb-4 text-center">
            Detalle del pedido
        </h2>

        <!-- Card principal -->
        <div class="card shadow border-0 mb-4">
            <div class="card-body">

                <!-- ICONO + NÚMERO DE PEDIDO -->
                <div class="d-flex align-items-center mb-3">
                    <div class="rounded-circle bg-primary text-white d-flex align-items-center justify-content-center"
                         style="width:50px;height:50px;">
                        <i class="fa-solid fa-receipt fs-4"></i>
                    </div>

                    <div class="ms-3">
                        <h4 class="m-0 fw-bold">
                            Pedido: <asp:Label ID="lblNumero" runat="server" />
                        </h4>
                        <small class="text-muted">
                            Fecha: <asp:Label ID="lblFecha" runat="server" />
                        </small>
                    </div>
                </div>

                <hr />

                <!-- Información del pedido -->
                <div class="row mb-3">

                    <div class="col-md-6 mb-2">
                        <p class="m-0">
                            <strong>Estado:</strong>
                            <span class="badge bg-info text-dark">
                                <asp:Label ID="lblEstado" runat="server" />
                            </span>
                        </p>
                    </div>

                    <div class="col-md-6 mb-2">
                        <p class="m-0">
                            <strong>Método de pago:</strong>
                            <asp:Label ID="lblPago" runat="server" CssClass="fw-bold" />
                        </p>
                    </div>

                    <div class="col-md-12 mt-2">
                        <p class="m-0">
                            <strong>Dirección de envío:</strong>
                            <asp:Label ID="lblDireccionEnvio" runat="server" />
                        </p>
                    </div>
                </div>

                <hr />

                <!-- Tabla de ítems -->
                <h5 class="fw-bold mb-3">Productos</h5>

                <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered"
                    HeaderStyle-CssClass="table-primary" RowStyle-CssClass="align-middle">

                    <Columns>
                        <asp:BoundField DataField="Titulo" HeaderText="Libro" />

                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />

                        <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio" DataFormatString="{0:C2}" />

                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C2}" />
                    </Columns>

                </asp:GridView>

                <div class="text-end mt-3">
                    <h4 class="fw-bold text-success">
                        Total: $ <asp:Label ID="lblTotal" runat="server" />
                    </h4>
                </div>

            </div>
        </div>

        <!-- Botón volver -->
        <div class="text-center mt-3">
            <a href="MisPedidos.aspx" class="btn btn-secondary px-4">
                Volver a mis pedidos
            </a>
        </div>

    </div>

</asp:Content>