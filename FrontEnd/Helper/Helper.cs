using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd
{
    public static class Helper
    {
        public static List<int> ListarAnios()
        {
            int añoInicio = 2024;
            int añoActual = DateTime.Now.Year;

            List<int> listAnio = Enumerable.Range(añoInicio, añoActual - añoInicio + 1)
                .OrderByDescending(a => a)
                .ToList();

            return listAnio;
        }

        public static List<string> ListarMeses()
        {
            List<string> listMeses = new List<string>
            {
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Setiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            };

            return listMeses;
        }
    }
}