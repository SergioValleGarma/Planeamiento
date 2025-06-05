using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class BloqueoViewModel
    {
        public int iCodigoDependencia { get; set; }
        public string vDependencia { get; set; }
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
    }

    public class DetalleBloqueo
    {
        public int iCodigoDependencia { get; set; }
        public int Mes { get; set; }
        public bool Bloqueado { get; set; }
    }

    public class BloqueoCreateViewModel
    {

        [Required]
        public int Anio { get; set; }
        
        [Required]
        public int Codigo_Modulo { get; set; }
        
        public List<DetalleBloqueo> DetalleBloqueo { get; set; }
        
        public int iCodUsuarioAccion { get; set; }
    }
}
