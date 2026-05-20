<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="E_Commerce_Bookstore.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Fondo del card en rojo muy clarito */
        .error-card {
            background-color: #ffe5e5 !important;   /* rojo suave */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5 pt-5">
        <div class="row justify-content-center">
            <div class="col-md-8">

                <div class="card shadow-sm border-0 error-card">
                    <div class="card-body text-center py-5">

                        <h1 class="fw-bold text-danger mb-3">
                            ¡Ups! Algo salió mal...
                        </h1>

                        <asp:Label ID="lblError" runat="server" CssClass="lead mb-4 text-danger" />

                        <p class="lead mb-4">
                            <br />
                            Te pedimos disculpas por las molestias.
                        </p>

                        <a href="Default.aspx" class="btn btn-primary btn-lg">
                            Volver al inicio
                        </a>

                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>