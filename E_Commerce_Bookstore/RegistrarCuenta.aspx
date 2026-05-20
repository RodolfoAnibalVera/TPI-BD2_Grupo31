<%@ Page Title="Registrar Cuenta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarCuenta.aspx.cs" Inherits="E_Commerce_Bookstore.RegistrarCuenta" UnobtrusiveValidationMode="None"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">

        <h2 class="mb-4">Crear cuenta</h2>

        <asp:Label ID="lblMensajeRegistro" runat="server" CssClass="text-danger"></asp:Label>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            CssClass="text-danger" HeaderText="Corregí estos errores:" />

        <div class="row">
            <div class="col-md-6">

                <!-- NOMBRE -->
                <div class="mb-3">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                        ErrorMessage="Ingresá tu nombre" CssClass="text-danger" Display="Dynamic"/>
                </div>

                <!-- APELLIDO -->
                <div class="mb-3">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido"
                        ErrorMessage="Ingresá tu apellido" CssClass="text-danger" Display="Dynamic"/>
                </div>

                <!-- DNI -->
                <div class="mb-3">
                    <label for="txtDni" class="form-label">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDni"
                        ErrorMessage="Ingresá tu DNI" CssClass="text-danger" Display="Dynamic"/>
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDni"
                        ValidationExpression="^\d+$"
                        ErrorMessage="El DNI debe ser numérico"
                        CssClass="text-danger" Display="Dynamic"/>
                </div>

                <!-- EMAIL -->
                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Ingresá tu email" CssClass="text-danger" Display="Dynamic"/>
                </div>

                <!-- TELEFONO -->
                <div class="mb-3">
                    <label for="txtTelefono" class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"/>
                </div>

            </div>

            <div class="col-md-6">

                <!-- DIRECCION -->
                <div class="mb-3">
                    <label for="txtDireccion" class="form-label">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control"/>
                </div>

                <!-- CP -->
                <div class="mb-3">
                    <label for="txtCP" class="form-label">Código postal</label>
                    <asp:TextBox ID="txtCP" runat="server" CssClass="form-control"/>
                </div>

                <!-- CONTRASEÑA -->
                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                        TextMode="Password"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Ingresá una contraseña" CssClass="text-danger" Display="Dynamic"/>
                </div>

                <!-- CONFIRMAR -->
                <div class="mb-3">
                    <label for="txtConfirmarPassword" class="form-label">Repetir contraseña</label>
                    <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="form-control"
                        TextMode="Password"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmarPassword"
                        ErrorMessage="Repetí la contraseña" CssClass="text-danger" Display="Dynamic"/>
                    <asp:CompareValidator runat="server" ControlToValidate="txtConfirmarPassword"
                        ControlToCompare="txtPassword"
                        ErrorMessage="Las contraseñas no coinciden"
                        CssClass="text-danger" Display="Dynamic"/>
                </div>

                <!-- BOTON -->
                <div class="mb-3">
                    <asp:Button ID="btnRegistrarme" runat="server" Text="Registrarme"
                        CssClass="btn btn-success" OnClick="btnRegistrarme_Click"/>
                </div>

                <div class="mb-3">
                    <asp:HyperLink ID="lnkVolverLogin" runat="server"
                        NavigateUrl="~/MiCuenta.aspx">
                        Volver a iniciar sesión
                    </asp:HyperLink>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
