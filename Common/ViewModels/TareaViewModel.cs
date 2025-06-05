using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class TareaViewModel
    {
        public int Id_Tarea { get; set; }
        public string Desagregacion { get; set; }
        public int Id_Proceso { get; set; }
        public string Proceso { get; set; }
        public int Cod_Unidad_Medida { get; set; }
        public string Unidad_Medida { get; set; }
        public bool CePlan { get; set; }
        public int Ene { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Abr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Ago { get; set; }
        public int Set { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dic { get; set; }
        public int Total { get; set; }
    }

    public class DetalleTarea
    {
        public int Mes { get; set; }
        public int Cantidad { get; set; }
    }

    public class TareaCreateViewModel
    {
        public int Id_Tarea { get; set; }
        
        [Required]
        public int Id_Actividad { get; set; }

        [Required(ErrorMessage = "El campo desegragación de la fila es obligatorio")]
        public string Desagregacion { get; set; }

        [Required(ErrorMessage = "El código de proceso de la fila es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El código de proceso no debe ser nulo")]
        public int Codigo_Proceso { get; set; }

        [Required(ErrorMessage = "La unidad de medida de la fila es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "La unidad de medida no debe ser nulo")]
        public int Codigo_Unidad_Medida { get; set; }
        public bool CePlan { get; set; }
        public List<DetalleTarea> listaMeses { get; set; }
        public int iCodUsuarioAccion { get; set; }
    }
}
