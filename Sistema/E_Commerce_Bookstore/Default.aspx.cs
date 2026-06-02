using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CargarBannerDestacados();
                CargarRecomendados();
            }
            catch
            {
                Response.Redirect("Error.aspx", false);
            }
        }

        private void CargarBannerDestacados()
        {
            LibroNegocio negocio = new LibroNegocio();
            string imagenPorDefecto = "https://placehold.co/400x600?text=Libro";

            List<Libro> listaBest = negocio.ListarBestSellers();
            if (listaBest != null && listaBest.Count > 0)
            {
                Libro l = listaBest[0];

                lblBestSellerTitulo.Text = l.Titulo;
                lblBestSellerPrecio.Text = l.PrecioVenta.ToString("N2");

                if (!string.IsNullOrEmpty(l.ImagenUrl))
                    imgBestSeller.ImageUrl = l.ImagenUrl;
                else
                    imgBestSeller.ImageUrl = imagenPorDefecto;
            }

            List<Libro> listaPop = negocio.ListarPopulares();
            if (listaPop != null && listaPop.Count > 0)
            {
                Libro l = listaPop[0];

                lblPopularTitulo.Text = l.Titulo;
                lblPopularPrecio.Text = l.PrecioVenta.ToString("N2");

                if (!string.IsNullOrEmpty(l.ImagenUrl))
                    imgPopular.ImageUrl = l.ImagenUrl;
                else
                    imgPopular.ImageUrl = imagenPorDefecto;
            }

            List<Libro> listaOfer = negocio.ListarOfertas();
            if (listaOfer != null && listaOfer.Count > 0)
            {
                Libro l = listaOfer[0];

                lblOfertaTitulo.Text = l.Titulo;
                lblOfertaPrecio.Text = l.PrecioVenta.ToString("N2");

                if (!string.IsNullOrEmpty(l.ImagenUrl))
                    imgOferta.ImageUrl = l.ImagenUrl;
                else
                    imgOferta.ImageUrl = imagenPorDefecto;
            }
        }

        private void CargarRecomendados()
        {
            CargarNovedad();
            CargarMasVendido();
            CargarComic();
        }

        private void CargarNovedad()
        {
            try
            {
                LibroNegocio negocio = new LibroNegocio();
                List<Libro> lista = negocio.Listar();

                if (lista != null && lista.Count > 0)
                {
                    Libro ultimo = lista.OrderByDescending(l => l.Id).First();

                    lblRecNovedadTitulo.Text = ultimo.Titulo;
                    lblRecNovedadPrecio.Text = ultimo.PrecioVenta.ToString("N2");

                    if (!string.IsNullOrEmpty(ultimo.ImagenUrl))
                        imgRecNovedad.ImageUrl = ultimo.ImagenUrl;
                    else
                        imgRecNovedad.ImageUrl = "https://placehold.co/400x600?text=Libro";
                }
            }
            catch
            {
                lblRecNovedadTitulo.Text = "Sin datos";
            }
        }

        private void CargarMasVendido()
        {
            try
            {
                LibroNegocio negocio = new LibroNegocio();
                List<Libro> listaPop = negocio.ListarPopulares();

                if (listaPop != null && listaPop.Count > 0)
                {
                    Libro masVendido = listaPop[0];

                    lblRecMasVendidoTitulo.Text = masVendido.Titulo;
                    lblRecMasVendidoPrecio.Text = masVendido.PrecioVenta.ToString("N2");

                    if (!string.IsNullOrEmpty(masVendido.ImagenUrl))
                        imgRecMasVendido.ImageUrl = masVendido.ImagenUrl;
                    else
                        imgRecMasVendido.ImageUrl = "https://placehold.co/400x600?text=Libro";
                }
            }
            catch
            {
                lblRecMasVendidoTitulo.Text = "Sin datos";
            }
        }

        private void CargarComic()
        {
            try
            {
                LibroNegocio negocio = new LibroNegocio();
                List<Libro> todos = negocio.Listar();

                int idCategoriaComics = todos
                    .Where(l => l.Categoria != null && !string.IsNullOrEmpty(l.Categoria.Nombre))
                    .Where(l =>
                    {
                        string nombre = l.Categoria.Nombre.Trim().ToLower();
                        return nombre == "comic" || nombre == "comics" || nombre == "cómic" || nombre == "cómics";
                    })
                    .Select(l => l.Categoria.Id)
                    .FirstOrDefault();

                if (idCategoriaComics > 0)
                {
                    List<Libro> comics = negocio.ListarPorCategoria(idCategoriaComics);

                    if (comics != null && comics.Count > 0)
                    {
                        Libro comic = comics.OrderByDescending(l => l.Id).First();

                        lblRecComicTitulo.Text = comic.Titulo;
                        lblRecComicPrecio.Text = comic.PrecioVenta.ToString("N2");

                        if (!string.IsNullOrEmpty(comic.ImagenUrl))
                            imgRecComic.ImageUrl = comic.ImagenUrl;
                        else
                            imgRecComic.ImageUrl = "https://placehold.co/400x600?text=Comic";

                        return;
                    }
                }

                lblRecComicTitulo.Text = "No hay comics";
            }
            catch
            {
                lblRecComicTitulo.Text = "Sin datos";
            }
        }

    }
}