using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class SeguimientoTareaViewModel
    {
        public Cab_SeguimientoTareaViewModel Cab_Tarea { get; set; }
        public IEnumerable<Det_SeguimientoTareaViewModel> Detalle { get; set; }
    }

    public class Cab_SeguimientoTareaViewModel 
    {
        public int iCodTarea_Act { get; set; }
        public string Desagregacion { get; set; }
        public string Proceso { get; set; }
        public string Unidad_Medida { get; set; }
        public bool bCePlan { get; set; }
    }

    public class Det_SeguimientoTareaViewModel 
    {
        public int iCodTarea_Act { get; set; }
        public int iCodDetTarea_Act { get; set; }
        public int Mes_Programado { get; set; }
        public int Cantidad_Programado { get; set; }
        public int Cantidad_Seguimiento { get; set; }
        public string Justificacion { get; set; }
    }

    public class Det_SeguimientoTareaCreateViewModel
    {
        public int iCodDetTarea_Act { get; set; }
        
        [Required]
        public int iCodTarea_Act { get; set; }
        
        [Required]
        public int nMes { get; set; }
        
        [Required(ErrorMessage = "La justificación es obligatoria")]
        public string nvJustificacion { get; set; }
        
        [Required(ErrorMessage = "La cantidad de seguimiento es obligatorio")]
        public int nCantidad { get; set; }
    }
}
