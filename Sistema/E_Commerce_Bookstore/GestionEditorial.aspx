<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionEditorial.aspx.cs" Inherits="E_Commerce_Bookstore.GestionEditorial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Gestión de Editoriales</h3>

    <div class="row">
        <div class="col-md-4">
            <label>Id</label>
            <asp:TextBox ID="txtId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

            <label>Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>

            <label>País</label>
            <asp:TextBox ID="txtPais" runat="server" CssClass="form-control"></asp:TextBox>

            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success mt-2" OnClick="btnAgregar_Click" />
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning mt-2" OnClick="btnModificar_Click" />
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger mt-2" OnClick="btnEliminar_Click" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-secondary mt-2" Text="Limpiar" OnClick="btnLimpiar_Click" />
            <br />
            <asp:Label ID="lblMensaje" runat="server" CssClass="mt-2"></asp:Label>
        </div>

        <div class="col-md-8">
            <div class="row mb-3">
                <div class="col-md-4">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar editorial..." />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnBuscarEdi" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscarEdi_Click" />
                </div>
            </div>

            <asp:GridView ID="dgvEditorial" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                DataKeyNames="Id" OnSelectedIndexChanged="dgvEditorial_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField HeaderText="Id" DataField="Id" />
                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                    <asp:BoundField HeaderText="País" DataField="Pais" />
                    <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" />
                </Columns>
            </asp:GridView>
        </div>

    </div>

</asp:Content>
