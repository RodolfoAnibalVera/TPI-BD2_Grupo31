<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisPedidos.aspx.cs" Inherits="E_Commerce_Bookstore.MisPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4">

        <h2 class="mb-4 text-center fw-bold text-primary">Mis pedidos</h2>

        <asp:Repeater ID="repPedidos" runat="server">
            <ItemTemplate>

                <div class="card mb-3 shadow-sm border-0">
                    <div class="card-body">

                        <!-- Header: icono + número de pedido -->
                        <div class="d-flex align-items-center mb-3">
                            <div class="rounded-circle bg-primary text-white d-flex align-items-center justify-content-center"
                                style="width: 45px; height: 45px;">
                                <i class="fa-solid fa-box fs-5"></i>
                            </div>

                            <div class="ms-3">
                                <h5 class="m-0 fw-bold text-dark">Pedido: <%# Eval("NumeroPedido") %></h5>
                                <small class="text-muted">Realizado el <%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %></small>
                            </div>
                        </div>

                        <hr />

                        <!-- Detalles -->
                        <p class="mb-2">
                            <strong>Total:</strong>
                            <span class="text-success fw-bold">$ <%# Eval("Subtotal") %></span>
                        </p>

                        <p class="mb-2">
                            <strong>Estado:</strong>
                            <span class="badge bg-warning text-dark">
                                <%# Eval("Estado") %>
                            </span>
                        </p>

                       <p class="mb-2">
                            <strong>Dirección de envío:</strong>
                            <span>
                                 <%# string.IsNullOrEmpty(Eval("DireccionEnvio") as string)
                                    ? "Retiro en local"
                                    : Eval("DireccionEnvio").ToString() %>
                            </span>
                       </p>


                        <!-- Botón -->
                        <div class="mt-3">
                            <a href='DetallePedido.aspx?id=<%# Eval("Id") %>'
                                class="btn btn-primary btn-sm px-3">Ver detalle
                            </a>
                        </div>

                    </div>
                </div>

            </ItemTemplate>
        </asp:Repeater>

        <!-- Botón Seguir comprando -->
        <div class="text-center my-4">
            <a id="btnVolver" runat="server" class="btn btn-primary px-4 py-2">Seguir comprando
            </a>

        </div>


        <!-- Mensaje cuando no hay pedidos -->
        <asp:Label ID="lblSinPedidos" runat="server"
            Text="Todavía no tenés compras registradas."
            CssClass="text-center d-block mt-4 text-muted fs-5"
            Visible="false"></asp:Label>

    </div>

</asp:Content>
