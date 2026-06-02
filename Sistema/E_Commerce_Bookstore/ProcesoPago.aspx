<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcesoPago.aspx.cs" Inherits="E_Commerce_Bookstore.ProcesoPago" UnobtrusiveValidationMode="None"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container my-4">

        <h2 class="mb-3">Método de pago</h2>

        <!-- LISTA DE METODOS -->
        <asp:RadioButtonList ID="rblMetodo" runat="server" AutoPostBack="true"
            OnSelectedIndexChanged="rblMetodo_SelectedIndexChanged">
            <asp:ListItem Value="TRANSFERENCIA">Transferencia bancaria</asp:ListItem>
            <asp:ListItem Value="EFECTIVO">Pago en efectivo</asp:ListItem>
            <asp:ListItem Value="DEBITO">Tarjeta de débito</asp:ListItem>
            <asp:ListItem Value="CREDITO">Tarjeta de crédito</asp:ListItem>
        </asp:RadioButtonList>

        <hr />

        <!-- TRANSFERENCIA -->
        <asp:Panel ID="pnlTransferencia" runat="server" CssClass="mt-3" Visible="false">
    <h4>Transferencia bancaria</h4>
    <p>Alias: <strong>LIBRO.TIENDA.PAGO</strong></p>
    <p>CBU: <strong>00012345000987654321</strong></p>

    <p class="mt-3">
        Total a pagar:
        <strong><asp:Label ID="lblMontoTransferencia" runat="server"></asp:Label></strong>
    </p>

    <!-- DATOS PARA ENVIAR EL COMPROBANTE -->
    <div class="alert alert-info mt-3">
        <p class="mb-1">Una vez realizada la transferencia, enviá el comprobante a:</p>
        <p class="mb-0">
            Email: <strong>contacto@bookcomicstore.com</strong><br />
            WhatsApp: <strong>+34 600 123 456</strong>
        </p>
    </div>
</asp:Panel>


        <!-- EFECTIVO -->
        <asp:Panel ID="pnlEfectivo" runat="server" CssClass="mt-3" Visible="false">
            <h4>Pago en efectivo</h4>
            <p>Podés abonar en el local al retirar el pedido.</p>
        </asp:Panel>

        <!-- TARJETA (DEBITO O CREDITO, REUTILIZADO) -->
        <asp:Panel ID="pnlTarjeta" runat="server" CssClass="mt-3" Visible="false">
    <h4>Datos de la tarjeta</h4>

    <!-- NUMERO DE TARJETA -->
    <div class="mb-2">
        <label>Número de tarjeta</label>
        <asp:TextBox ID="txtNumeroTarjeta" runat="server" CssClass="form-control"
            MaxLength="16" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNumeroTarjeta"
            ErrorMessage="Ingresá el número de tarjeta"
            CssClass="text-danger" Display="Dynamic" />
        <asp:RegularExpressionValidator runat="server"
            ControlToValidate="txtNumeroTarjeta"
            ValidationExpression="^\d{16}$"
            ErrorMessage="La tarjeta debe tener exactamente 16 números"
            CssClass="text-danger" Display="Dynamic" />
    </div>

    <!-- NOMBRE DEL TITULAR -->
    <div class="mb-2">
        <label>Nombre y apellido del titular</label>
        <asp:TextBox ID="txtNombreTarjeta" runat="server" CssClass="form-control"
            MaxLength="23" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombreTarjeta"
            ErrorMessage="Ingresá el nombre del titular"
            CssClass="text-danger" Display="Dynamic" />
        <asp:RegularExpressionValidator runat="server"
            ControlToValidate="txtNombreTarjeta"
            ValidationExpression="^[A-Za-zÁÉÍÓÚÑáéíóúñ ]{1,23}$"
            ErrorMessage="Usá solo letras y hasta 23 caracteres"
            CssClass="text-danger" Display="Dynamic" />
    </div>

    <div class="row">
        <!-- VENCIMIENTO -->
        <div class="col-md-6 mb-2">
            <label>Vencimiento (MM/AA)</label>
            <asp:TextBox ID="txtVencimientoTarjeta" runat="server" CssClass="form-control"
                MaxLength="5" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtVencimientoTarjeta"
                ErrorMessage="Ingresá el vencimiento"
                CssClass="text-danger" Display="Dynamic" />
            <asp:RegularExpressionValidator runat="server"
                ControlToValidate="txtVencimientoTarjeta"
                ValidationExpression="^(0[1-9]|1[0-2])\/\d{2}$"
                ErrorMessage="Formato inválido. Usá MM/AA"
                CssClass="text-danger" Display="Dynamic" />
            <asp:CustomValidator ID="valVencimientoTarjeta" runat="server"
                ControlToValidate="txtVencimientoTarjeta"
                OnServerValidate="ValidarVencimientoTarjeta"
                ErrorMessage="La tarjeta está vencida"
                CssClass="text-danger" Display="Dynamic" />
        </div>

        <!-- CODIGO DE SEGURIDAD -->
        <div class="col-md-6 mb-2">
            <label>Código de seguridad (CVV)</label>
            <asp:TextBox ID="txtCodigoSeguridad" runat="server" CssClass="form-control"
                TextMode="Password" MaxLength="3" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCodigoSeguridad"
                ErrorMessage="Ingresá el código de seguridad"
                CssClass="text-danger" Display="Dynamic" />
            <asp:RegularExpressionValidator runat="server"
                ControlToValidate="txtCodigoSeguridad"
                ValidationExpression="^\d{3}$"
                ErrorMessage="El código debe tener exactamente 3 números"
                CssClass="text-danger" Display="Dynamic" />
        </div>
    </div>
</asp:Panel>


        <asp:Button ID="btnConfirmarPago" runat="server" CssClass="btn btn-primary mt-4"
            Text="Confirmar pago" OnClick="btnConfirmarPago_Click" />

    </div>

</asp:Content>