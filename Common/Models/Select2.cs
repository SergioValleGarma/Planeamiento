using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    interface mSelect2<T>
    {
        List<T> item { get; set; }
        int total_count { get; set; }
    }
}
