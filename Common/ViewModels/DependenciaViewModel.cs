using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class DependenciaViewModel
    {
        public string vPadre { get; set; }
        public int iCodigoDependencia { get; set; }
        public string vDependencia { get; set; }
    }

    public class DependeciaGrouped
    {
        public string vPadre { get; set; }
        public List<DependenciaViewModel> lstDependencia {get;set;}
    }
}
