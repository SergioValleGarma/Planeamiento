using Microsoft.AspNet.Identity;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace Common.Auth
{
    public class CurrentUser
    {
        public long iCodUsuario { get; set; }
        public long iCodPersona { get; set; }      
        public long iCodPerfil { get; set; }
        public string vNombrePerfil { get; set; }
        public string vNombreUsuario { get; set; }        
        public string vNombres { get; set; }
        public string vApePaterno { get; set; }
        public string vApeMaterno { get; set; }
        public List<string> Roles { get; set; }
        public CurrentUser()
        {
            Roles = new List<string>();
        }
        public bool IsAdmin()
        {
            return Roles.Contains(RoleNames.Admin);
        }

        public bool IsOperador()
        {
            return Roles.Contains(RoleNames.Operador);
        }

        public bool IsSupervisor()
        {
            return Roles.Contains(RoleNames.Supervisor);
        }
        public class CurrentUserHelper
        {
            public static CurrentUser Get
            {
                get
                {
                    var user = HttpContext.Current.User;

                    if (user == null)
                    {
                        return null;
                    }
                    else if (string.IsNullOrEmpty(user.Identity.GetUserId()))
                    {
                        return null;
                    }

                    var jUser = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.UserData).Value;

                    var x = JsonConvert.DeserializeObject<CurrentUser>(jUser);
                    return JsonConvert.DeserializeObject<CurrentUser>(jUser);
                }
            }
        }
    }
}