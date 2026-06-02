<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecuperarPassword.aspx.cs" Inherits="E_Commerce_Bookstore.RecuperarPassword" UnobtrusiveValidationMode="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">
        <h2 class="mb-4">Recuperar contraseña</h2>

        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger"></asp:Label>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            CssClass="text-danger" HeaderText="Corregí estos errores:" />

        <!-- EMAIL -->
        <div class="mb-3">
            <label for="txtEmail" class="form-label">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                ErrorMessage="Ingresá tu email" CssClass="text-danger" Display="Dynamic" />
        </div>

        <!-- NUEVA CONTRASEÑA -->
        <div class="mb-3">
            <label for="txtNuevaPassword" class="form-label">Nueva contraseña</label>
            <asp:TextBox ID="txtNuevaPassword" runat="server" CssClass="form-control"
                TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNuevaPassword"
                ErrorMessage="Ingresá la nueva contraseña" CssClass="text-danger" Display="Dynamic" />
        </div>

        <!-- CONFIRMAR CONTRASEÑA -->
        <div class="mb-3">
            <label for="txtConfirmarPassword" class="form-label">Repetir contraseña</label>
            <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="form-control"
                TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmarPassword"
                ErrorMessage="Repetí la contraseña" CssClass="text-danger" Display="Dynamic" />
            <asp:CompareValidator runat="server"
                ControlToValidate="txtConfirmarPassword"
                ControlToCompare="txtNuevaPassword"
                ErrorMessage="Las contraseñas no coinciden"
                CssClass="text-danger" Display="Dynamic" />
        </div>

        <!-- BOTÓN -->
        <div class="mb-3">
            <asp:Button ID="btnRecuperar" runat="server" Text="Guardar nueva contraseña"
                CssClass="btn btn-primary" OnClick="btnRecuperar_Click" />
        </div>

        <div class="mb-3">
            <asp:HyperLink ID="lnkVolverLogin" runat="server"
                NavigateUrl="~/MiCuenta.aspx">
                Volver a iniciar sesión
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>
