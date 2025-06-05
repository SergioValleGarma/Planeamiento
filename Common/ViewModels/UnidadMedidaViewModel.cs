using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class UnidadMedidaViewModel : mSelect2<UnidadMedida>
    {
        public List<UnidadMedida> item { get; set; }
        public int total_count { get; set; }
    }
}
