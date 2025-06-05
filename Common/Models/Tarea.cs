using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Tarea : Auditoria
    {
        public int iCodTarea { get; set; }
        public int iCodActividadIncrementable { get; set; }
        public string nvDesagregacion { get; set; }
        public int iCodProceso { get; set; }
        public int iCodUnidadMedida { get; set; }
        public List<DetTarea> DetalleTarea { get; set; }
        public bool bCePlan { get; set; }
    }

    public class DetTarea : Auditoria
    {
        public int iCodDetTarea { get; set; }
        public int iCodTarea { get; set; }
        public int nMes { get; set; }
        public int nCantidad { get; set; }
    }
}
