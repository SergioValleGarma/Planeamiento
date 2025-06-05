using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ReporteSemaforo
    {
        public string vSiglas { get; set; }
        public string nvCodAccion { get; set; }
        public string nvCodFinalidadPresupuestal { get; set; }
        public string nvDenominacionActividad { get; set; }
        public string Unidad_Medida { get; set; }
        public int Meta_Fisica_Anual { get; set; }
        public decimal Meta_Financiera_Anual { get; set; }
        public int Meta_Programada_Hasta { get; set; }
        public int Ejecucion_Fisica_Hasta { get; set; }
        public decimal Ejecucion_Financiera_Hasta { get; set; }
    }

    public class ParametrosReporteSemaforo
    { 
        public decimal Rango_Primer_Valor { get; set; }
        public decimal Rango_Segundo_Valor { get; set; }
        public string Clasificacion { get; set; }
    }
}
