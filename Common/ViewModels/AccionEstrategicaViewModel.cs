using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class AccionEstrategicaViewModel
    {
        public int Id_Accion { get; set; }
        public string Codigo_Accion { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCompleta => $"{Codigo_Accion} {Descripcion}";

    }
}
