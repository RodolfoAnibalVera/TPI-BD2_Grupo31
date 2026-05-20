<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionCategoria.aspx.cs" Inherits="E_Commerce_Bookstore.GestionCategoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mt-3">Gestión de Categorías</h2>

    <asp:Label ID="lbMensaje" runat="server" CssClass="fw-bold mt-3"></asp:Label>

    <div class="row mt-3">
        <div class="col-md-4">
            <label class="form-label">ID</label>
            <asp:TextBox ID="txtId" CssClass="form-control" runat="server" ReadOnly="true" />
        </div>

        <div class="col-md-4">
            <label class="form-label">Nombre</label>
            <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
        </div>

        <div class="col-md-4">
            <label class="form-label">Activo</label><br />
            <asp:CheckBox ID="chkActivo" runat="server" />
        </div>
    </div>

    <div class="mt-3">
        <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Guardar" OnClick="btnGuardar_Click" />
        <asp:Button ID="btnModificar" runat="server" CssClass="btn btn-warning ms-2" Text="Modificar" OnClick="btnModificar_Click" />
        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click1" />
        <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-secondary ms-2" Text="Limpiar" OnClick="btnLimpiar_Click" />
    </div>

    <hr />


    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar categoría..." />
        </div>
        <div class="col-md-2">
            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                <asp:ListItem Text="Activas" Value="Activas" />
                <asp:ListItem Text="Inactivas" Value="Inactivas" />
                <asp:ListItem Text="Todas" Value="Todas" />
            </asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
        </div>
    </div>

    <asp:GridView ID="dgvCategorias" runat="server" AutoGenerateColumns="false"
        CssClass="table table-striped table-bordered mt-3"
        OnSelectedIndexChanged="dgvCategorias_SelectedIndexChanged"
        DataKeyNames="Id">

        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

            <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" />
        </Columns>

    </asp:GridView>

</asp:Content>
