<%@ Page Title="Proceso de Envío" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcesoEnvio.aspx.cs" Inherits="E_Commerce_Bookstore.ProcesoEnvio" UnobtrusiveValidationMode="None" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="c1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4" style="max-width:900px">

  <!-- ==================== DATO DE CONTACTO ==================== -->
<h3 class="mb-3">Dato de contacto</h3>
<div class="mb-3">

    <label class="form-label">Email</label>
    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />

    <small class="text-muted d-block mt-1">
        Ingresá tu correo para enviarte la factura de tu compra.
    </small>

    <!-- EMAIL OBLIGATORIO -->
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
        ErrorMessage="El email es obligatorio" CssClass="text-danger" Display="Dynamic" />

    <!-- VALIDACIÓN DE FORMATO EMAIL -->
    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail"
        ErrorMessage="Formato de email inválido" CssClass="text-danger" Display="Dynamic"
        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
</div>

<!-- CHECKBOX PARA NOTIFICACIONES -->
<div class="form-check mb-3">
    <asp:CheckBox ID="chkNotificaciones" runat="server" CssClass="form-check-input" />
    <label class="form-check-label" for="chkNotificaciones">
        Deseo recibir notificaciones y ofertas por email.
    </label>
</div>

<hr class="my-4" />


    <!-- ==================== DATOS DE ENTREGA ==================== -->
    <asp:Panel ID="pnlEntrega" runat="server" CssClass="border rounded p-3 mt-3">
    <h3 class="mb-3">Datos de entrega</h3>
    <div class="row g-3">
      <div class="col-md-6">
        <label class="form-label">Nombre</label>
        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
          ErrorMessage="Nombre requerido" CssClass="text-danger" Display="Dynamic" />
      </div>

      <div class="col-md-6">
        <label class="form-label">Apellido</label>
        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido"
          ErrorMessage="Apellido requerido" CssClass="text-danger" Display="Dynamic" />
      </div>

      <div class="col-md-3">
        <label class="form-label">Código Postal</label>
        <asp:TextBox ID="txtCP" runat="server" CssClass="form-control" MaxLength="10" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCP"
          ErrorMessage="Código postal requerido" CssClass="text-danger" Display="Dynamic" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCP"
          ErrorMessage="Solo números" CssClass="text-danger" Display="Dynamic"
          ValidationExpression="^\d{1,10}$" />
      </div>

          <div class="col-md-9">
        <label class="form-label">Dirección (calle y número)</label>
        <asp:TextBox ID="txtCalle" runat="server" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCalle"
          ErrorMessage="Dirección requerida" CssClass="text-danger" Display="Dynamic" />
      </div>


      <div class="col-md-4">
        <label class="form-label">Departamento</label>
        <asp:TextBox ID="txtDepto" runat="server" CssClass="form-control" />
      </div>

      <div class="col-md-4">
        <label class="form-label">Barrio</label>
        <asp:TextBox ID="txtBarrio" runat="server" CssClass="form-control" />
      </div>

      <div class="col-md-4">
        <label class="form-label">Ciudad</label>
        <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCiudad"
          ErrorMessage="Ciudad requerida" CssClass="text-danger" Display="Dynamic" />
      </div>
    </div>
    </asp:Panel>

    <!-- ==================== CHECKBOX FACTURACIÓN ==================== -->
    <div class="form-check mt-4">
      <asp:CheckBox ID="chkFacturacion" runat="server" CssClass="form-check-input"
        AutoPostBack="true" OnCheckedChanged="chkFacturacion_CheckedChanged" />
      <label class="form-check-label" for="chkFacturacion">
        Usar datos de facturación diferentes
      </label>
    </div>

    <!-- ==================== DATOS DE FACTURACIÓN ==================== -->
    <asp:Panel ID="pnlFacturacion" runat="server" CssClass="border rounded p-3 mt-3" Visible="false">
      <h5 class="mb-3">Datos de facturación</h5>
      <div class="row g-3">
        <div class="col-md-6">
          <label class="form-label">Nombre</label>
          <asp:TextBox ID="txtFacNombre" runat="server" CssClass="form-control" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFacNombre"
            ErrorMessage="Nombre requerido" CssClass="text-danger" Display="Dynamic" />
        </div>

        <div class="col-md-6">
          <label class="form-label">Apellido</label>
          <asp:TextBox ID="txtFacApellido" runat="server" CssClass="form-control" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFacApellido"
            ErrorMessage="Apellido requerido" CssClass="text-danger" Display="Dynamic" />
        </div>

                <div class="col-md-9">
          <label class="form-label">Dirección (calle y número)</label>
          <asp:TextBox ID="txtFacCalle" runat="server" CssClass="form-control" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFacCalle"
            ErrorMessage="Dirección requerida" CssClass="text-danger" Display="Dynamic" />
        </div>


        <div class="col-md-4">
          <label class="form-label">Departamento</label>
          <asp:TextBox ID="txtFacDepto" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-4">
          <label class="form-label">Barrio</label>
          <asp:TextBox ID="txtFacBarrio" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-4">
          <label class="form-label">Ciudad</label>
          <asp:TextBox ID="txtFacCiudad" runat="server" CssClass="form-control" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFacCiudad"
            ErrorMessage="Ciudad requerida" CssClass="text-danger" Display="Dynamic" />
        </div>

        <div class="col-md-4">
          <label class="form-label">Código Postal</label>
          <asp:TextBox ID="txtFacCP" runat="server" CssClass="form-control" MaxLength="10" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFacCP"
            ErrorMessage="Código postal requerido" CssClass="text-danger" Display="Dynamic" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFacCP"
            ErrorMessage="Solo números" CssClass="text-danger" Display="Dynamic"
            ValidationExpression="^\d{1,10}$" />
        </div>
      </div>
    </asp:Panel>

    <!-- ==================== VALIDACIONES Y BOTONES ==================== -->
    <asp:ValidationSummary ID="vsEnvio" runat="server"
      CssClass="text-danger mt-3" />

    <div class="mt-4 d-flex gap-2">
      <a href="Carrito.aspx" class="btn btn-outline-secondary">Volver al carrito</a>
      <asp:Button ID="btnContinuar" runat="server"
        Text="Continuar a Pago"
        CssClass="btn btn-primary"
        OnClick="btnContinuar_Click" />
    </div>

  </div>
</asp:Content>