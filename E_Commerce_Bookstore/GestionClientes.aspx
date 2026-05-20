<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionClientes.aspx.cs" Inherits="E_Commerce_Bookstore.GestionClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center my-4">Gestión de Clientes</h2>

    <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mb-3"></asp:Label>

     <hr />

    <!-- FILTRO -->
    <div class="row mb-3">
        <div class="col-md-3">
            <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" placeholder="Buscar Nombre, Apellido, DNI, Email o Telefono"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100"
                OnClick="btnBuscar_Click" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnLimpiar1" runat="server" Text="Limpiar" CssClass="btn btn-secondary w-100"
                OnClick="btnLimpiar1_Click" />
        </div>
    </div>

    <!-- Grilla -->
    <asp:GridView ID="dgvClientes" runat="server" AutoGenerateColumns="False"
        CssClass="table table-bordered table-hover"
        DataKeyNames="Id" OnSelectedIndexChanged="dgvClientes_SelectedIndexChanged">

        <Columns>
            <asp:BoundField HeaderText="ID" DataField="Id" />
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
            <asp:BoundField HeaderText="Apellido" DataField="Apellido" />
            <asp:BoundField HeaderText="DNI" DataField="DNI" />
            <asp:BoundField HeaderText="Email" DataField="Email" />
            <asp:BoundField HeaderText="Telefono" DataField="Telefono" />
            <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" />
        </Columns>

    </asp:GridView>

    <hr />

    <!-- Formulario -->
    <div class="row">

        <div class="col-md-3">
            <asp:Label Text="ID" runat="server" />
            <asp:TextBox ID="txtId" runat="server" CssClass="form-control" ReadOnly="true" />
        </div>

        <div class="col-md-3">
            <asp:Label Text="Nombre" runat="server" />
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-3">
            <asp:Label Text="Apellido" runat="server" />
            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-3">
            <asp:Label Text="DNI" runat="server" />
            <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-4">
            <asp:Label Text="Email" runat="server" />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
        </div>

        <div class="col-md-4">
            <asp:Label Text="Id Usuario" runat="server" />
            <asp:TextBox ID="txtIdUsuario" runat="server" CssClass="form-control" ReadOnly="true" />
            <asp:Button ID="btnIrGestionUsuario" runat="server" Text="Agregar nuevo usuario"
                CssClass="btn btn-primary mt-2"
                OnClick="btnIrGestionUsuario_Click" />

        </div>

        <div class="col-md-4">
            <asp:Label Text="Telefono" runat="server" />
            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-6">
            <asp:Label Text="Direccion" runat="server" />
            <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-2">
            <asp:Label Text="CP" runat="server" />
            <asp:TextBox ID="txtCP" runat="server" CssClass="form-control" />
        </div>
    </div>

    <br />

    <!-- Botones -->
    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success" OnClick="btnAgregar_Click" />
    <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click" />
    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click" />
    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />

</asp:Content>
