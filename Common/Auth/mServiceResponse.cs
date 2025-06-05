using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth
{
    public class mServiceResponse
    {
        public string message { get; set; }
        public string statusCode { get; set; }
        public bool bResponse { get; set; }

        [JsonProperty("person")]
        public ApiUser person { get; set; }
    }
}
