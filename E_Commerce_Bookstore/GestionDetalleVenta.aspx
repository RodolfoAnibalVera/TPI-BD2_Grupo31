<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionDetalleVenta.aspx.cs" Inherits="E_Commerce_Bookstore.GestionDetalleVenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="titulo">Gestión Detalle del Pedido</div>

        <!-- Mensajes -->
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>

        <!-- Información del pedido -->
        <div class="card mb-4">
            <h4>Información del Pedido</h4>

            <div class="row mb-3">
                <div class="col-md-4">
                    <label>ID Pedido</label>
                    <asp:TextBox ID="txtIdPedido" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>

        <!-- Formulario CRUD -->
        <div class="card mb-4">
            <h4>Detalle</h4>

            <div class="row mb-3">
                <div class="col-md-3">
                    <label>ID Detalle</label>
                    <asp:TextBox ID="txtIdDetalle" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-md-4">
                    <label>Libro</label>
                    <asp:DropDownList ID="ddlLibros" CssClass="form-select" runat="server"></asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label>Cantidad</label>
                    <asp:TextBox ID="txtCantidad" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <label>Precio Unitario</label>
                    <asp:TextBox ID="txtPrecioUnitario" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-4">
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success w-100" OnClick="btnAgregar_Click" />
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning w-100" OnClick="btnModificar_Click" />
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger w-100" OnClick="btnEliminar_Click" />
                </div>
            </div>

        </div>

        <!-- Grilla -->
        <div class="card">
            <h4>Artículos del Pedido</h4>

            <asp:GridView ID="dgvDetalles" runat="server"
                AutoGenerateColumns="False" CssClass="table table-bordered"
                OnSelectedIndexChanged="dgvDetalles_SelectedIndexChanged1"
                DataKeyNames="Id">
                <Columns>
                    <asp:BoundField HeaderText="ID" DataField="Id" />
                    <asp:BoundField HeaderText="Título" DataField="Titulo" />
                    <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" />
                    <asp:BoundField HeaderText="Precio" DataField="PrecioUnitario" DataFormatString="{0:C}" />
                    <asp:BoundField HeaderText="Subtotal" DataField="Subtotal" DataFormatString="{0:C}" />
                    <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" />
                </Columns>
            </asp:GridView>


        </div>

    </div>

</asp:Content>
