<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionVentas.aspx.cs" Inherits="E_Commerce_Bookstore.GestionVentas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center mt-4 mb-4">Gestión de Pedidos</h2>

    <!-- FORMULARIO -->
    <div class="card p-4 mb-5">

        <h4 class="mb-4">Detalle del Pedido</h4>
        <asp:Label ID="lbMensaje" runat="server" CssClass="fw-bold mt-3 text-center d-block"></asp:Label>

        <div class="row">

            <div class="col-md-3">
                <label>ID</label>
                <asp:TextBox ID="txtId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>ID Cliente</label>
                <asp:TextBox ID="txtIdCliente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>Cliente</label>
                <asp:TextBox ID="txtClienteNombre" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>Número Pedido</label>
                <asp:TextBox ID="txtNumeroPedido" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>Fecha</label>
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>Estado</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem>Pendiente</asp:ListItem>
                    <asp:ListItem>Enviado</asp:ListItem>
                    <asp:ListItem>Entregado</asp:ListItem>
                    <asp:ListItem>Cancelado</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <label>Subtotal</label>
                <asp:TextBox ID="txtSubtotal" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>Total</label>
                <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-6 mt-3">
                <label>Dirección Envío</label>
                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

        </div>

        <!-- BOTONES -->
        <div class="mt-4">
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success me-2"
                OnClick="btnAgregar_Click" />

            <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning me-2"
                OnClick="btnModificar_Click" />

            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger"
                OnClick="btnEliminar_Click" />

            <asp:Button ID="btnDetallePedido" runat="server" Text="Ver detalle.." CssClass="btn btn-primary"
                OnClick="btnDetallePedido_Click1" />
        </div>
    </div>

    <!-- FILTRO -->
    <div class="row mb-3">
        <div class="col-md-3">
            <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" placeholder="Buscar por cliente o nro pedido..."></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100"
                OnClick="btnBuscar_Click" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary w-100"
                OnClick="btnLimpiar_Click" />
        </div>
    </div>

    <!-- GRILLA -->
    <asp:GridView ID="dgvPedidos" runat="server" AutoGenerateColumns="False"
        CssClass="table table-bordered table-hover text-center"
        DataKeyNames="Id"
        OnSelectedIndexChanged="dgvPedidos_SelectedIndexChanged">

        <Columns>
            <asp:BoundField HeaderText="ID" DataField="Id" />
            <asp:BoundField HeaderText="Cliente" DataField="ClienteNombre" />
            <asp:BoundField HeaderText="N° Pedido" DataField="NumeroPedido" />
            <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField HeaderText="Estado" DataField="Estado" />
            <asp:BoundField HeaderText="Total" DataField="Total" DataFormatString="{0:C}" />

            <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" />
        </Columns>
    </asp:GridView>
</asp:Content>
