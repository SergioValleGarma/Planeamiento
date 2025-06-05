using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class CabeceraActividadOperativaViewModel
    {
        public int Id_Codigo_Actividad_Cat { get; set; }
        public string Numero_Actividad { get; set; }
    }

    public class CabeceraActividadOperativaCreateViewModel
    {
        [Required(ErrorMessage = "El código de la actividad es obligatorio")]
        [RegularExpression(@"^AOI\d{11}$", ErrorMessage = "El valor debe comenzar con 'AOI' en mayúsculas seguido de 11 números.")]
        public string Numero_Actividad { get; set; }

        [Required(ErrorMessage = "La dependencia es requerida")]
        public int Catalogo_AO_Codigo_Dependencia { get; set; }

        [Required(ErrorMessage = "El año es requerido")]
        public int nAnio { get; set; }
    }
}
