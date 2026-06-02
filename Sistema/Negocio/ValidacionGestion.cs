using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Negocio
{
    public class ValidacionGestion
    {
        public List<string> ValidarLibro(Dominio.Libro libro)
        {
            List<string> errores = new List<string>();

            // Validaciones de texto obligatorio
            if (string.IsNullOrWhiteSpace(libro.Titulo))
                errores.Add("El campo 'Título' es obligatorio.");

            if (libro.Autor == null || libro.Autor.Id <= 0)
                errores.Add("El campo 'Autor' es obligatorio.");

            if (libro.Editorial == null || libro.Editorial.Id <= 0)
                errores.Add("El campo 'Editorial' es obligatorio.");

            if (string.IsNullOrWhiteSpace(libro.ISBN))
                errores.Add("El campo 'ISBN' es obligatorio.");
            else if (!Regex.IsMatch(libro.ISBN, @"^\d{10}(\d{3})?$"))
                errores.Add("El ISBN debe tener 10 o 13 dígitos.");

            if (ExisteISBN(libro.ISBN) && libro.Id == 0)
                errores.Add("Ya existe un articulo con el mismo ISBN!!");

            // Año válido
            if (libro.AnioEdicion < 1800 || libro.AnioEdicion > DateTime.Now.Year)
                errores.Add("El año de edición no es válido.");

            // Páginas y stock
            if (libro.Paginas <= 0)
                errores.Add("El número de páginas debe ser mayor a 0.");

            if (libro.Stock <= 0 || libro.Stock == null)
                errores.Add("El stock no puede ser negativo o cero.");

            // Precios
            if (libro.PrecioCompra <= 0)
                errores.Add("El precio de compra debe ser mayor a 0.");

            if (libro.PrecioVenta <= 0)
                errores.Add("El precio de venta debe ser mayor a 0.");

            if (libro.PrecioVenta < libro.PrecioCompra)
                errores.Add("El precio de venta no puede ser menor que el de compra.");

            // Porcentaje de ganancia
            if (libro.PorcentajeGanancia < 0 || libro.PorcentajeGanancia > 100)
                errores.Add("El porcentaje de ganancia debe estar entre 0% y 100%.");

            // URL de imagen
            if (!string.IsNullOrWhiteSpace(libro.ImagenUrl))
            {
                Uri uriResult;
                bool urlValida = Uri.TryCreate(libro.ImagenUrl, UriKind.Absolute, out uriResult)
                                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (!urlValida)
                    errores.Add("La URL de la imagen no es válida.");
            }

            // Categoría
            if (libro.Categoria == null || libro.Categoria.Id <= 0)
                errores.Add("Debe seleccionar una categoría válida.");

            return errores;
        }

        public bool ExisteISBN(string isbn)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM LIBROS WHERE ISBN = @isbn");
                datos.setearParametro("@isbn", isbn);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    return (int)datos.Lector[0] > 0;

                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool EmailExiste(string email, int? idExcluido = null)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT COUNT(*) FROM USUARIOS WHERE Email = @email";

                // Si estás modificando un usuario, evita validar contra sí mismo
                if (idExcluido != null)
                {
                    consulta += " AND Id <> @id";
                }

                datos.setearConsulta(consulta);
                datos.setearParametro("@email", email);

                if (idExcluido != null)
                    datos.setearParametro("@id", idExcluido);

                int count = (int)datos.ejecutarScalar();

                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error validando email: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}