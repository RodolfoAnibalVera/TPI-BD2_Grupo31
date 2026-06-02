using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class CarritoCompra
    {
        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public string CookieId { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public bool Activo { get; set; } = true;
        public List<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}