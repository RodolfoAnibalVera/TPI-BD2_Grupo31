<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="E_Commerce_Bookstore.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <!-- Aquí puedes agregar <style> o <script> específicos de esta página -->
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="text-center">
        <h1 class="mb-4">Bienvenido a Book&Comic Store</h1>
        <p class="lead">Explora los mejores libros y cómics del momento 📚🦸‍♂️</p>
        <!-- BANNER DESTACADOS -->
<div id="bannerDestacados" class="carousel slide mb-4" data-bs-ride="carousel">
    <!-- PUNTITOS -->
    <div class="carousel-indicators">
        <button type="button" data-bs-target="#bannerDestacados" data-bs-slide-to="0" class="active"></button>
        <button type="button" data-bs-target="#bannerDestacados" data-bs-slide-to="1"></button>
        <button type="button" data-bs-target="#bannerDestacados" data-bs-slide-to="2"></button>
    </div>

    <div class="carousel-inner">

        <!-- SLIDE 1 - BEST SELLER -->
        <div class="carousel-item active">
            <div class="row align-items-center bg-dark text-light p-4 rounded">
                <div class="col-md-8">
                    <span class="badge bg-warning text-dark mb-2">Best Seller</span>
                    <h2 class="mb-2">
                        <asp:Label ID="lblBestSellerTitulo" runat="server" Text="Best seller destacado"></asp:Label>
                    </h2>
                    <h4 class="mb-3">
                        $ <asp:Label ID="lblBestSellerPrecio" runat="server" Text="0"></asp:Label>
                    </h4>
                    <p>Los libros más leídos por nuestros lectores.</p>
                    <a href="BestSeller.aspx" class="btn btn-light">Ver Best Sellers</a>
                </div>
                <div class="col-md-4 text-center">
                    <asp:Image ID="imgBestSeller" runat="server" CssClass="img-fluid rounded shadow" AlternateText="Best seller" />
                </div>
            </div>
        </div>

        <!-- SLIDE 2 - POPULARES -->
        <div class="carousel-item">
            <div class="row align-items-center bg-primary text-light p-4 rounded">
                <div class="col-md-8">
                    <span class="badge bg-light text-primary mb-2">Popular</span>
                    <h2 class="mb-2">
                        <asp:Label ID="lblPopularTitulo" runat="server" Text="Libro popular"></asp:Label>
                    </h2>
                    <h4 class="mb-3">
                        $ <asp:Label ID="lblPopularPrecio" runat="server" Text="0"></asp:Label>
                    </h4>
                    <p>Lo que más está comprando la gente.</p>
                    <a href="Populares.aspx" class="btn btn-light">Ver Populares</a>
                </div>
                <div class="col-md-4 text-center">
                    <asp:Image ID="imgPopular" runat="server" CssClass="img-fluid rounded shadow" AlternateText="Libro popular" />
                </div>
            </div>
        </div>

        <!-- SLIDE 3 - OFERTAS -->
        <div class="carousel-item">
            <div class="row align-items-center bg-success text-light p-4 rounded">
                <div class="col-md-8">
                    <span class="badge bg-light text-success mb-2">Oferta</span>
                    <h2 class="mb-2">
                        <asp:Label ID="lblOfertaTitulo" runat="server" Text="Libro en oferta"></asp:Label>
                    </h2>
                    <h4 class="mb-3">
                        $ <asp:Label ID="lblOfertaPrecio" runat="server" Text="0"></asp:Label>
                    </h4>
                    <p>Precios especiales por tiempo limitado.</p>
                    <a href="Ofertas.aspx" class="btn btn-light">Ver Ofertas</a>
                </div>
                <div class="col-md-4 text-center">
                    <asp:Image ID="imgOferta" runat="server" CssClass="img-fluid rounded shadow" AlternateText="Libro en oferta" />
                </div>
            </div>
        </div>

    </div>

    <!-- FLECHAS -->
    <button class="carousel-control-prev" type="button" data-bs-target="#bannerDestacados" data-bs-slide="prev">
        <span class="carousel-control-prev-icon"></span>
    </button>
    <button class="carousel-control-next" type="button" data-bs-target="#bannerDestacados" data-bs-slide="next">
        <span class="carousel-control-next-icon"></span>
    </button>
</div>
<!-- FIN BANNER DESTACADOS -->
<!-- ==================== CATEGORÍAS DESTACADAS ==================== -->
        <hr class="my-5" />

        <h3 class="text-center mb-4">Explorá por Categorías</h3>

        <div class="row text-center">
            <div class="col-md-3 mb-4">
                <div class="p-3 border rounded shadow-sm h-100">
                    <div style="font-size: 40px;">📘</div>
                    <h5>Libros</h5>
                    <a href="Catalogo.aspx" class="btn btn-outline-primary btn-sm mt-2">Ver más</a>
                </div>
            </div>

            <div class="col-md-3 mb-4">
                <div class="p-3 border rounded shadow-sm h-100">
                    <div style="font-size: 40px;">🦸‍♂️</div>
                    <h5>Cómics</h5>
                    <a href="Catalogo.aspx" class="btn btn-outline-primary btn-sm mt-2">Ver más</a>
                </div>
            </div>

            <div class="col-md-3 mb-4">
                <div class="p-3 border rounded shadow-sm h-100">
                    <div style="font-size: 40px;">🎓</div>
                    <h5>Educación</h5>
                    <a href="Catalogo.aspx" class="btn btn-outline-primary btn-sm mt-2">Ver más</a>
                </div>
            </div>

            <div class="col-md-3 mb-4">
                <div class="p-3 border rounded shadow-sm h-100">
                    <div style="font-size: 40px;">🎲</div>
                    <h5>Fantasía y Juegos</h5>
                    <a href="Catalogo.aspx" class="btn btn-outline-primary btn-sm mt-2">Ver más</a>
                </div>
            </div>
        </div>
        <!-- ==================== FIN CATEGORÍAS DESTACADAS ==================== -->


        <!-- ==================== RECOMENDADOS PARA VOS ==================== -->
<hr class="my-5" />

<h3 class="text-center mb-4">Recomendados para vos</h3>

<div class="row">

    <!-- ==================== TARJETA 1: NOVEDADES ==================== -->
    <div class="col-md-4 mb-3">
        <div class="card h-100 shadow-sm">

            <asp:Image ID="imgRecNovedad" runat="server"
                       CssClass="card-img-top"
                       ImageUrl="https://placehold.co/400x600?text=Libro" />

            <div class="card-body">
                <h6 class="text-primary fw-bold mb-2">Novedades</h6>

                <h5 class="card-title">
                    <asp:Label ID="lblRecNovedadTitulo" runat="server" Text="Libro recomendado" />
                </h5>

                <p class="card-text fw-bold">
                    $ <asp:Label ID="lblRecNovedadPrecio" runat="server" Text="0,00" />
                </p>

                <a href="Catalogo.aspx" class="btn btn-primary">Ver más</a>
            </div>
        </div>
    </div>

    <!-- ==================== TARJETA 2: CÓMIC RECOMENDADO ==================== -->
    <div class="col-md-4 mb-3">
        <div class="card h-100 shadow-sm">

            <asp:Image ID="imgRecComic" runat="server"
                       CssClass="card-img-top"
                       ImageUrl="https://placehold.co/400x600?text=Comic" />

            <div class="card-body">
                <h6 class="text-primary fw-bold mb-2">Cómic recomendado</h6>

                <h5 class="card-title">
                    <asp:Label ID="lblRecComicTitulo" runat="server" Text="Cómic" />
                </h5>

                <p class="card-text fw-bold">
                    $ <asp:Label ID="lblRecComicPrecio" runat="server" Text="0,00" />
                </p>

                <a href="Catalogo.aspx" class="btn btn-primary">Ver más</a>
            </div>
        </div>
    </div>

    <!-- ==================== TARJETA 3: MÁS VENDIDO DEL MES ==================== -->
    <div class="col-md-4 mb-3">
        <div class="card h-100 shadow-sm">

            <asp:Image ID="imgRecMasVendido" runat="server"
                       CssClass="card-img-top"
                       ImageUrl="https://placehold.co/400x600?text=Libro" />

            <div class="card-body">
                <h6 class="text-primary fw-bold mb-2">Más vendido del mes</h6>

                <h5 class="card-title">
                    <asp:Label ID="lblRecMasVendidoTitulo" runat="server" Text="Título" />
                </h5>

                <p class="card-text fw-bold">
                    $ <asp:Label ID="lblRecMasVendidoPrecio" runat="server" Text="0,00" />
                </p>

                <a href="Catalogo.aspx" class="btn btn-primary">Ver más</a>
            </div>
        </div>
    </div>

</div>
<!-- ==================== FIN RECOMENDADOS ==================== -->




        <!-- ==================== BENEFICIOS DE LA TIENDA ==================== -->
        <hr class="my-5" />

        <div class="row text-center">
            <div class="col-md-4 mb-3">
                <div class="p-3 bg-light rounded shadow-sm">
                    <div style="font-size: 40px;">🚚</div>
                    <h5 class="mt-2">Envíos a domicilio</h5>
                    <p>Rápido, seguro y confiable.</p>
                </div>
            </div>

            <div class="col-md-4 mb-3">
                <div class="p-3 bg-light rounded shadow-sm">
                    <div style="font-size: 40px;">💳</div>
                    <h5 class="mt-2">Pagos seguros</h5>
                    <p>Aceptamos tarjeta, transferencia y efectivo.</p>
                </div>
            </div>

            <div class="col-md-4 mb-3">
                <div class="p-3 bg-light rounded shadow-sm">
                    <div style="font-size: 40px;">📦</div>
                    <h5 class="mt-2">Retiro en sucursal</h5>
                    <p>Compra online y retiralo cuando quieras.</p>
                </div>
            </div>
        </div>
        <!-- ==================== FIN BENEFICIOS ==================== -->

        <a href="Catalogo.aspx" class="btn btn-primary btn-lg mt-3">Ver Catálogo</a>
    </div>
</asp:Content>



