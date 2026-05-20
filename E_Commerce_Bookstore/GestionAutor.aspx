<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionAutor.aspx.cs" Inherits="E_Commerce_Bookstore.GestionAutor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Gestión de Autores</h2>

    <asp:Label ID="lbMensaje" runat="server" />

    <hr />

    <div class="row">
        <div class="col-md-4">
            <label>Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />

            <label class="mt-2">Nacionalidad</label>
            <asp:TextBox ID="txtNacionalidad" runat="server" CssClass="form-control" />

            <asp:HiddenField ID="txtId" runat="server" />

            <div class="mt-3">
                <asp:Button ID="btnGuardarAutor" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardarAutor_Click" />
                <asp:Button ID="btnModificarAutor" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificarAutor_Click" />
                <asp:Button ID="btnEliminarAutor" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminarAutor_Click" />
            </div>
        </div>

        <div class="col-md-8">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control mb-2" Placeholder="Buscar autor..." />
            <asp:Button ID="btnBuscarAutor" runat="server" Text="Buscar" CssClass="btn btn-primary mb-3" OnClick="btnBuscarAutor_Click" />

            <asp:GridView ID="dgvAutores" runat="server" AutoGenerateColumns="false" CssClass="table table-striped"
                OnSelectedIndexChanged="dgvAutores_SelectedIndexChanged" DataKeyNames="Id">

                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Nacionalidad" HeaderText="Nacionalidad" />
                    <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
