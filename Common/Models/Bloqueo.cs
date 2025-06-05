using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Bloqueo: Auditoria
    {
        public int iCodBloqueo { get; set; }
        public int iCodModulo { get; set; }
        public int nAnio { get; set; }
        public List<DetBloqueo> DetalleBloqueo { get; set; }
    }

    public class DetBloqueo : Auditoria
    {
        public int iCodigoDependencia { get; set; }
        public int nMes { get; set; }
        public bool bBloqueado { get; set; }
    }
}
