using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class ObjetivoEstrategicoViewModel
    {
        public int Id_Objetivo { get; set; }
        public string Codigo_Objetivo { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCompleta => $"{Codigo_Objetivo} {Descripcion}";
    }
}
