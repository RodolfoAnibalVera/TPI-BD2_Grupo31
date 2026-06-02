<%@ Page Title="Gestión de Artículos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionProducto.aspx.cs" Inherits="E_Commerce_Bookstore.GestionProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .preview-img {
            max-width: 140px;
            height: auto;
            border: 1px solid #ccc;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }

        .card {
            border-radius: 1rem;
        }

        .form-label {
            font-weight: 600;
        }

        .alert-warning {
            background-color: #fff3cd;
            border-color: #ffeeba;
            color: #856404;
            border-radius: 0.5rem;
            padding: 1rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="my-5">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="card shadow border-0">
                    <div class="card-header bg-success text-white text-center py-3 rounded-top">
                        <h3 class="mb-0"><i class="bi bi-book-half"></i>Gestión de Artículos</h3>
                    </div>

                    <div class="card-body px-4 py-5">
                        <div class="row">
                            <!-- Columna izquierda: formulario -->
                            <div class="col-md-9">
                                <div class="row g-3">

                                    <div class="col-md-2">
                                        <label for="txtId" class="form-label">ID</label>
                                        <asp:TextBox ID="txtId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="col-md-5">
                                        <label for="txtTitulo" class="form-label">Título</label>
                                        <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Ej: Harry Potter y la Piedra Filosofal"></asp:TextBox>
                                    </div>

                                    <div class="col-md-5">
                                        <label for="lbAutor" class="form-label">Autor</label>
                                        <asp:DropDownList ID="ddlAutor" runat="server" CssClass="form-select"></asp:DropDownList>
                                        <asp:Button ID="btnGestionAutor"  CssClass="btn btn-success me-2 mt-2" runat="server" Text="Nuevo autor" OnClick="btnGestionAutor_Click" />
                                    </div>

                                    <div class="col-md-12">
                                        <label for="txtDescripcion" class="form-label">Descripción</label>
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Descripción del producto..."></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <label for="txtISBN" class="form-label">ISBN</label>
                                        <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <label for="txtIdioma" class="form-label">Idioma</label>
                                        <asp:TextBox ID="txtIdioma" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <label for="txtAnioEdicion" class="form-label">Año de Edición</label>
                                        <asp:TextBox ID="txtAnioEdicion" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3">
                                        <label for="txtPaginas" class="form-label">Páginas</label>
                                        <asp:TextBox ID="txtPaginas" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3">
                                        <label for="txtStock" class="form-label">Stock</label>
                                        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3 d-flex align-items-end">
                                        <div class="form-check">
                                            <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input me-2" Checked="true" />
                                            <label for="chkActivo" class="form-check-label">Activo</label>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <label for="ddlCategoria" class="form-label">Categoría</label>
                                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnNuevaCat"  CssClass="btn btn-success me-2 mt-2" runat="server" Text="Nueva categoria" OnClick="btnNuevaCat_Click"/>
                                    </div>

                                    <div class="col-md-6">
                                        <label for="ddlEditorial" class="form-label">Editorial</label>
                                        <asp:DropDownList ID="ddlEditorial" runat="server" CssClass="form-select"></asp:DropDownList>
                                        <asp:Button ID="btnNuevaEdi"  CssClass="btn btn-success me-2 mt-2" runat="server" Text="Nueva Editorial" OnClick="btnNuevaEdi_Click" />
                                    </div>

                                    <div class="col-md-6">
                                        <label for="txtImagenUrl" class="form-label">URL de Imagen</label>
                                        <asp:TextBox ID="txtImagenUrl" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtImagenUrl_TextChanged" placeholder="https://..."></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <label for="txtPrecioCompra" class="form-label">Precio Compra</label>
                                        <asp:TextBox ID="txtPrecioCompra" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <label for="txtPrecioVenta" class="form-label">Precio Venta</label>
                                        <asp:TextBox ID="txtPrecioVenta" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <label for="txtPorcentajeGanancia" class="form-label">% Ganancia</label>
                                        <asp:TextBox ID="txtPorcentajeGanancia" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <!-- Botones de gestion -->
                                    <div class="text-center mt-4">
                                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success me-2" OnClick="btnGuardar_Click" />
                                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning me-2" OnClick="btnModificar_Click" />
                                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary me-2" OnClick="btnLimpiar_Click" />
                                        <asp:Button ID="btnMostrarConfirmacion" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnMostrarConfirmacion_Click" />

                                        <!-- Advertencia "Eliminar" -->
                                        <!-- Panel de confirmación (inicialmente oculto) -->
                                        <asp:Panel ID="pnlConfirmacion" runat="server" Visible="false" CssClass="alert alert-warning text-center mt-3">
                                            <strong>⚠️ Advertencia:</strong> Está a punto de eliminar del catalogo este libro.
                                            <br />
                                            <span>¿Desea continuar?</span>
                                            <div class="mt-3">
                                                <asp:Button ID="btnConfirmarEliminar" runat="server" Text="Sí, eliminar" CssClass="btn btn-danger me-2" OnClick="btnEliminar_Click" />
                                                <asp:Button ID="btnCancelarEliminar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarEliminar_Click" />
                                            </div>
                                        </asp:Panel>
                                    </div>

                                    <!-- Mensajes de gestiones y errores -->
                                    <asp:Label ID="lbMensaje" runat="server" CssClass="fw-bold mt-3 text-center d-block"></asp:Label>
                                </div>
                            </div>

                            <!-- Columna derecha: miniatura -->
                            <div class="col-md-3 text-center">
                                <asp:Image ID="imgPortada" runat="server" CssClass="preview-img mt-3" />
                                <small class="text-muted d-block mt-2">Vista previa</small>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Buscar -->

                <div class="card shadow border-0 mt-5 col-md-6 mb-3">
                    <div class="input-group">
                        <asp:TextBox ID="txtBuscar" class="form-control" placeholder="Ingrese palabra o letra.." runat="server"></asp:TextBox>
                        <asp:Button ID="btnBuscar" class="btn btn-primary" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>

                <!-- Filtro avanzado -->
                <div class="card shadow-sm p-4 mb-4">
                    <h4 class="mb-3">🔍 Filtro avanzado</h4>
                    <div class="row g-3 align-items-end">
                        <!-- Campo -->
                        <div class="col-md-2">
                            <label for="ddlCampo" class="form-label">Campo</label>
                            <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                                <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                <asp:ListItem Text="Título" Value="Titulo"></asp:ListItem>
                                <asp:ListItem Text="ISBN" Value="ISBN"></asp:ListItem>
                                <asp:ListItem Text="Precio Venta" Value="PrecioVenta"></asp:ListItem>
                                <asp:ListItem Text="Stock" Value="Stock"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Criterio -->
                        <div class="col-md-2">
                            <label for="ddlCriterio" class="form-label">Criterio</label>
                            <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Filtro -->
                        <div class="col-md-2">
                            <label for="txtFiltro" class="form-label">Filtro</label>
                            <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" placeholder="Ingrese valor..."></asp:TextBox>
                        </div>

                        <!-- Estado -->
                        <div class="col-md-2">
                            <label for="ddlEstado" class="form-label">Estado</label>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Todos" Value="*" />
                                <asp:ListItem Text="Activos" Value="1" />
                                <asp:ListItem Text="Inactivos" Value="0" />
                            </asp:DropDownList>
                        </div>

                        <!-- Botón Filtrar -->
                        <div class="col-md-1">
                            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary w-100 " OnClick="btnFiltrar_Click" />
                        </div>
                        <!-- Botón Limpiar -->
                        <div class="col-md-1">
                            <asp:Button ID="btnLimpiarFiltro" runat="server" Text="Limpiar" CssClass="btn btn-secondary w-100 ms-1" OnClick="btnLimpiarFiltro_Click" />
                        </div>
                    </div>
                </div>


                <!-- Tabla de libros -->
                <div class="card shadow border-0 mt-5">
                    <div class="card-header bg-dark text-white">
                        <h5 class="mb-0"><i class="bi bi-list"></i>Listado de Artículos</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="dgvArticulo" runat="server" CssClass="table table-hover table-bordered text-center align-middle"
                            AutoGenerateColumns="false" DataKeyNames="Id" OnSelectedIndexChanged="dgvArticulo_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="Id" />
                                <asp:BoundField HeaderText="Título" DataField="Titulo" />
                                <asp:BoundField HeaderText="ISBN" DataField="ISBN" />
                                <asp:BoundField HeaderText="Stock" DataField="Stock" />
                                <asp:BoundField HeaderText="Precio Compra" DataField="PrecioCompra" DataFormatString="{0:C}" />
                                <asp:BoundField HeaderText="Precio Venta" DataField="PrecioVenta" DataFormatString="{0:C}" />
                                <asp:BoundField HeaderText="% Ganancia" DataField="PorcentajeGanancia" />
                                <asp:BoundField DataField="IdCategoria" HeaderText="Categoría" Visible="false" />
                                <asp:CheckBoxField HeaderText="Activo" DataField="Activo" />
                                <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" HeaderText="Acción" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
