using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public static class Validaciones
    {
        public static bool EsEntero(string valor)
        {
            return int.TryParse(valor, out _);
        }

        public static bool EsDecimal(string valor)
        {
            return decimal.TryParse(valor, out _);
        }

        public static bool EsFecha(string valor)
        {
            return DateTime.TryParse(valor, out _);
        }

        public static bool NoVacio(string valor)
        {
            return !string.IsNullOrWhiteSpace(valor);
        }

        public static void Requerido(string valor, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new Exception($"El campo '{campo}' es obligatorio.");
        }

        public static void RequeridoNumero(string valor, string campo)
        {
            if (!int.TryParse(valor, out _))
                throw new Exception($"El campo '{campo}' debe ser un número válido.");
        }

        public static void RequeridoDecimal(string valor, string campo)
        {
            if (!decimal.TryParse(valor, out _))
                throw new Exception($"El campo '{campo}' debe ser un decimal válido.");
        }

        public static void RequeridoFecha(string valor, string campo)
        {
            if (!DateTime.TryParse(valor, out _))
                throw new Exception($"El campo '{campo}' debe ser una fecha válida.");
        }
    }
}
