using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ActividadOperativa_Actualizada : Auditoria
    {
        public int iCodActividadIncrementable { get; set; }
        public int? iCodTabActividadOperativa { get; set; }
        public int iCodUnidadMedida { get; set; }
        public string nvCodFinalidadPresupuestal { get; set; }
        public string nvDenominacionActividad { get; set; }
        public string nvDescripcionActividad { get; set; }
        public int iCodAccion { get; set; }
        public int iCodigoDependencia { get; set; }
        public int nAnio { get; set; }
    }
}
