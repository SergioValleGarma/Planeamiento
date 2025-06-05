using Newtonsoft.Json;

using System.Collections.Generic;

namespace Common.Auth
{
    public class ApiUser
    {
        [JsonProperty("iUsu")]
        public string iCodUsuario { get; set; }

        [JsonProperty(PropertyName = "uNom")]
        public string vUsuarioNombre { get; set; }

        [JsonProperty(PropertyName = "iTra")]
        public string iCodTrabajador { get; set; }

        [JsonProperty(PropertyName = "iDep")]
        public string iCodDependencia { get; set; }

        [JsonProperty(PropertyName = "bInt")]
        public bool bFlgUsuarioIntegrado { get; set; }

        [JsonProperty(PropertyName = "iPer")]
        public int iCodPersona { get; set; }

        [JsonProperty(PropertyName = "pNDo")]
        public string vPersonaNroDocumento { get; set; }

        [JsonProperty(PropertyName = "pNom")]
        public string vPersonaNombres { get; set; }

        [JsonProperty(PropertyName = "pAPa")]
        public string vPersonaApellidoPaterno { get; set; }

        [JsonProperty(PropertyName = "pAMa")]
        public string vPersonaApellidoMaterno { get; set; }

        [JsonProperty(PropertyName = "iEnt")]
        public string iCodEntidad { get; set; }

        [JsonProperty(PropertyName = "eRSo")]
        public string vEntidadRazonSocial { get; set; }

        [JsonProperty(PropertyName = "eRUC")]
        public string vEntidadRUC { get; set; }

        [JsonProperty(PropertyName = "pfls")]
        public List<ApiPerfiles> perfiles { get; set; }

        [JsonProperty(PropertyName = "opcs")]
        public List<ApiOpciones> opciones { get; set; }

        #region Token required
        public string nbf { get; set; }
        public string exp { get; set; }
        public string iat { get; set; }
        #endregion
    }
}
