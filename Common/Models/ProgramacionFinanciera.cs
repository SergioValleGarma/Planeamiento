using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ProgramacionFinanciera : Auditoria
    {
        public Int64 iCodProgramacionFinanciera { get; set; }
        public Int64 iCodDetProgamacionFinanciera { get; set; }
        public decimal nTechoActividadOperativa { get; set; }
        public int iCodActividad { get; set; }
        public List<DetProgramacionFinanciera> DetalleProgramacion { get; set; }
    }

    public class DetProgramacionFinanciera
    {
        public int iCodDetProgFin { get; set; }
        public int iCodProgramacionFinanciera { get; set; }
        public int nMes { get; set; }
        public decimal nCantidad { get; set; }
    }

}
