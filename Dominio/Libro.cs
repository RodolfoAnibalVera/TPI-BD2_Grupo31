using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string ISBN { get; set; }
        public string Idioma { get; set; }
        public int AnioEdicion { get; set; }
        public int Paginas { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public bool BestSeller { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PorcentajeGanancia { get; set; }

        public string ImagenUrl { get; set; }
        public Editorial Editorial { get; set; }
        public Autor Autor { get; set; }

        public Categoria Categoria { get; set; }
    }
}
