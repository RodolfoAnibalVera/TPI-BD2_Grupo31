<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MiCuenta.aspx.cs" Inherits="E_Commerce_Bookstore.MiCuenta" UnobtrusiveValidationMode="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">
        <h2 class="mb-4">Iniciar sesión</h2>

        <asp:Label ID="lblMensajeLogin" runat="server" CssClass="text-danger"></asp:Label>

        <div class="mb-3">
            <label for="txtEmailLogin" class="form-label">Email</label>
            <asp:TextBox ID="txtEmailLogin" runat="server" CssClass="form-control"
                TextMode="Email"></asp:TextBox>

            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmailLogin"
        ErrorMessage="Ingresá el email" CssClass="text-danger" Display="Dynamic" />
        </div>

        <div class="mb-3">
            <label for="txtPasswordLogin" class="form-label">Contraseña</label>
            <asp:TextBox ID="txtPasswordLogin" runat="server" CssClass="form-control"
                TextMode="Password"></asp:TextBox>

             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPasswordLogin"
        ErrorMessage="Ingresá la contraseña" CssClass="text-danger" Display="Dynamic" />
        </div>

        <div class="mb-3">
            <asp:Button ID="btnLogin" runat="server" Text="Iniciar sesión"
                CssClass="btn btn-primary"
                OnClick="btnLogin_Click" />
        </div>

        <div class="mb-3">
            <asp:HyperLink ID="lnkOlvidePass" runat="server"
                NavigateUrl="~/RecuperarPassword.aspx">
                ¿Olvidaste tu contraseña?
            </asp:HyperLink>
        </div>

        <hr />

        <div class="mt-3">
            <p>¿Todavía no tenés cuenta?</p>
            <asp:HyperLink ID="lnkRegistrarme" runat="server"
                NavigateUrl="~/RegistrarCuenta.aspx"
                CssClass="btn btn-outline-primary">
                Registrarme
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>