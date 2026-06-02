<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmacionCompra.aspx.cs" Inherits="E_Commerce_Bookstore.ConfirmacionCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4">
    <div class="row justify-content-center">
      <div class="col-12 col-md-10 col-lg-8">
        <div class="card border-0 shadow">

          <!-- Header con color -->
          <div class="card-header bg-primary text-white d-flex align-items-center">
            <span class="d-inline-flex align-items-center justify-content-center me-3
                         rounded-circle bg-white text-primary"
                  style="width:40px;height:40px;">
              <span class="fw-bold fs-5">✓</span>
            </span>

            <div class="flex-grow-1">
              <asp:Label ID="lblTitulo" runat="server" CssClass="h5 m-0 d-block"></asp:Label>
              <small class="opacity-75">Gracias por elegirnos</small>
            </div>

            <span class="badge bg-light text-primary ms-3">
              <asp:Label ID="lblMetodo" runat="server" Text=""></asp:Label>
            </span>
          </div>

          <!-- Cuerpo -->
          <div class="card-body p-4">

            <!-- Cuadro de mensaje dinámico -->
            <div id="boxMensaje" runat="server" class="alert mb-4">
              <asp:Label ID="lblMensaje" runat="server" />
            </div>

            <!-- DATOS DEL PEDIDO  -->
            <div class="border rounded p-3 mb-4 bg-light">
              <h5 class="mb-3">Detalles del pedido</h5>

              <p class="mb-1"><strong>Número de pedido:</strong> 
                <asp:Label ID="lblNumeroPedido" runat="server" />
              </p>

              <p class="mb-1"><strong>Fecha:</strong> 
                <asp:Label ID="lblFecha" runat="server" />
              </p>

              <p class="mb-1"><strong>Total:</strong> 
                <asp:Label ID="lblTotal" runat="server" />
              </p>

              <p class="mb-1"><strong>Estado:</strong> 
                <asp:Label ID="lblEstado" runat="server" />
              </p>

              <p class="mb-0"><strong>Dirección de envío:</strong> 
                <asp:Label ID="lblDireccionEnvio" runat="server" />
              </p>
            </div>

            <!-- Botones -->
            <div class="row g-2">
              <div class="col-sm">
                <a href="Catalogo.aspx" class="btn btn-outline-primary w-100">
                  Seguir comprando
                </a>
              </div>
              <div class="col-sm">
                <a href="MisPedidos.aspx" class="btn btn-success w-100">
                  Ver mis compras
                </a>
              </div>
            </div>

            <p class="text-muted small mt-3 mb-0">
              Ante cualquier consulta, escribinos y te ayudamos con tu pedido.
            </p>
          </div>

        </div>
      </div>
    </div>
  </div>
</asp:Content>

