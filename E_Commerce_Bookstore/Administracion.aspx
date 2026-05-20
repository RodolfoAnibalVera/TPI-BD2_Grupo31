<%@ Page Title="Panel de Administración" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administracion.aspx.cs" Inherits="E_Commerce_Bookstore.Administracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .admin-header {
            /*background: linear-gradient(135deg, #007bff, #6610f2);*/
            color: white;
            border-radius: 0.75rem;
            padding: 2rem 1rem;
            text-align: center;
            margin-bottom: 2rem;
            box-shadow: 0 4px 10px rgba(0,0,0,0.15);
        }

            .admin-header h1 {
                font-size: 2rem;
                font-weight: 600;
            }

        .dashboard-cards {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
            gap: 1.5rem;
        }

        .dashboard-card {
            background: #fff;
            border-radius: 1rem;
            padding: 2rem;
            text-align: center;
            box-shadow: 0 3px 8px rgba(0,0,0,0.1);
            transition: all 0.3s ease;
            cursor: pointer;
        }

            .dashboard-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 6px 12px rgba(0,0,0,0.2);
            }

            .dashboard-card i {
                font-size: 2.5rem;
                color: #007bff;
                margin-bottom: 1rem;
            }

            .dashboard-card h4 {
                font-weight: 600;
                margin-bottom: 0.5rem;
            }

            .dashboard-card p {
                color: #666;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">

        <div class="admin-header bg-success">
            <h1><i class="bi bi-speedometer2"></i>Panel de Administración</h1>

        </div>

        <div class="dashboard-cards">
            <!-- Gestión de Productos -->
            <a href="GestionProducto.aspx" class="text-decoration-none text-dark">
                <div class="dashboard-card">
                    <i class="bi bi-book-half"></i>
                    <h4>Gestión de Productos</h4>
                    <p>Agregar, modificar o eliminar libros del catálogo.</p>
                </div>
            </a>

            <!-- Gestión de Ventas -->
            <a href="GestionVentas.aspx" class="text-decoration-none text-dark">
                <div class="dashboard-card">
                    <i class="bi bi-cart-check"></i>
                    <h4>Gestión de Ventas</h4>
                    <p>Control y seguimiento de pedidos, facturación y estados.</p>
                </div>
            </a>

            <!-- Gestión de Clientes -->
            <a href="GestionClientes.aspx" class="text-decoration-none text-dark">
                <div class="dashboard-card">
                    <i class="bi bi-people"></i>
                    <h4>Gestión de Clientes</h4>
                    <p>Consulta y administración de clientes registrados.</p>
                </div>
            </a>

            <!-- Gestión de Usuarios -->
            <a href="GestionUsuario.aspx" class="text-decoration-none text-dark">
                <div class="dashboard-card">
                    <i class="bi bi-people"></i>
                    <h4>Gestión de Usuarios</h4>
                    <p>Consulta y administración de usuarios registrados.</p>
                </div>
            </a>

        </div>

    </div>
</asp:Content>
