using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class ProcesoViewModel : mSelect2<Proceso>
    {
        public List<Proceso> item { get; set; }
        public int total_count { get; set; }
    }
}
