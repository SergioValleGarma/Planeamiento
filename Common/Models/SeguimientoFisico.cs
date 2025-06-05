using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class SeguimientoFisico : Auditoria
    {
        public int iCodDetTarea_Act { get; set; }
        public int iCodTarea_Act { get; set; }
        public int nMes { get; set; }
        public string nvJustificacion { get; set; }
        public int nCantidad { get; set; }
    }
}
