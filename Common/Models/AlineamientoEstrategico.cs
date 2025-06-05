using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class AlineamientoEstrategico
    {
        public int Id_Objetivo_Estrategico { get; set; }
        public string Objetivo_Descripcion { get; set; }
        public int Id_Accion_Estrategico { get; set; }
        public string Accion_Descripcion { get; set; }
    }
}
