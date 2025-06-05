using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class  ActividadOperativaActualizadaViewModel
    {
        public int Id_Actividad_Incrementable_Act { get; set; }
        public int Id_Codigo_Actividad_Cat { get; set; }
        public string Codigo_Actividad { get; set; }
        public string Codigo_Finalidad_Presupuestal { get; set; }
        public string Denominacion { get; set; }
        public int Codigo_Objetivo { get; set; }
        public string Objetivo_Estrategico { get; set; }
        public string Descripcion_Objetivo { get; set; }
        public int Codigo_Accion { get; set; }
        public string Accion_Estrategico { get; set; }
        public string Descripcion_Accion { get; set; }
        public string Descripcion_Actividad { get; set; }
        public int Codigo_Unidad_Organizacion { get; set; }
        public string Dependencia { get; set; }
        public string Unidad_Organizacion { get; set; }
        public int Id_Unidad_Medida { get; set; }
        public string Unidad_Medida { get; set; }
        public int nAnio { get; set; }
    }

    public class ActividadOperativaActualizadaCreateViewModel
    {
        public int Codigo_Actividad_Incrementable { get; set; }

        public int? Id_Codigo_Actividad_Cat { get; set; }

        [Required(ErrorMessage = "La finalidad presupuestal es obligatorio")]
        public string Codigo_Finalidad_Presupuestal { get; set; }

        [Required(ErrorMessage = "La denominación es obligatorio")]
        public string Denominacion { get; set; }

        [Required(ErrorMessage = "El objetivo estratégico es obligatorio")]
        public int Cod_Objetivo_Est { get; set; }

        [Required(ErrorMessage = "La acción estratégica es obligatorio")]
        public int Cod_Accion_Est { get; set; }

        [Required(ErrorMessage = "La descripción de la actividad es obligatorio")]
        public string Descripcion_Actividad { get; set; }

        [Required(ErrorMessage = "La Unidad de Medidad es obligatoria")]
        public int Id_Unidad_Medida { get; set; }

        [Required]
        public int Cod_Unidad_Organica { get; set; }

        [Required]
        public int nAnio { get; set; }

        public int iCodUsuarioAccion { get; set; }
        public string Unidad_Medida { get; set; }
    }
}
