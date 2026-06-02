<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModificacionCliente.aspx.cs" Inherits="E_Commerce_Bookstore.ModificacionCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container d-flex justify-content-center mt-5">
        <div class="card shadow w-100" style="max-width: 800px;">
            <div class="card-body">
                <h2 class="text-center mb-4">Modificar mis datos</h2>
                <div class="row mb-2">
                    <label class="col-sm-4 col-form-label text-dark fw-semibold">Nombre:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                            ControlToValidate="txtNombre"
                            ErrorMessage="El nombre es obligatorio"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                    </div>
                </div>
                <div class="row mb-2">
                    <label class="col-sm-4 col-form-label text-dark fw-semibold">Apellido:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                            ControlToValidate="txtApellido"
                            ErrorMessage="El apellido es obligatorio"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                    </div>
                </div>
                <div class="row mb-2">
                    <label class="col-sm-4 col-form-label text-dark fw-semibold">DNI:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvDNI" runat="server"
                            ControlToValidate="txtDNI"
                            ErrorMessage="El DNI es obligatorio"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revDNI" runat="server"
                            ControlToValidate="txtDNI"
                            ValidationExpression="^\d+$"
                            ErrorMessage="El DNI debe contener solo números"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                    </div>
                </div>
                <div class="row mb-3 align-items-center">
                    <label for="txtEmail" class="col-sm-4 col-form-label fw-semibold text-dark">
                        Email:
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm" ReadOnly="true" />
                            <span class="input-group-text bg-light">
                                <i class="fa-solid fa-lock text-secondary"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="row mb-2">
                    <label class="col-sm-4 col-form-label text-dark fw-semibold">Teléfono:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                            ControlToValidate="txtTelefono"
                            ErrorMessage="El teléfono es obligatorio"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revTelefono" runat="server"
                            ControlToValidate="txtTelefono"
                            ValidationExpression="^\d+$"
                            ErrorMessage="El teléfono debe contener solo números"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                    </div>
                </div>
                <div class="row mb-2">
                    <label class="col-sm-4 col-form-label text-dark fw-semibold">Dirección:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvDireccion" runat="server"
                            ControlToValidate="txtDireccion"
                            ErrorMessage="La dirección es obligatoria"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                    </div>
                </div>
                <div class="row mb-4">
                    <label class="col-sm-4 col-form-label text-dark fw-semibold">Código Postal:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtCP" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvCP" runat="server"
                            ControlToValidate="txtCP"
                            ErrorMessage="El código postal es obligatorio"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revCP" runat="server"
                            ControlToValidate="txtCP"
                            ValidationExpression="^\d+$"
                            ErrorMessage="El código postal debe contener solo números"
                            CssClass="text-danger fw-bold"
                            Display="Dynamic" />
                    </div>
                </div>

                <div class="d-flex justify-content-between">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar cambios" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                    <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="MiPerfil.aspx" CssClass="btn btn-secondary">Cancelar</asp:HyperLink>
                </div>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger fw-bold mb-3 d-block" Visible="false" />
            </div>
        </div>
    </div>
</asp:Content>
