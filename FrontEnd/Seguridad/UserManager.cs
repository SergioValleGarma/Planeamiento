using Common.Auth;

using Newtonsoft.Json;

using System.Security.Claims;
using System.Threading;
using System.Web;

namespace FrontEnd.Seguridad
{
    public class UserManager
    {
        public static bool ExistUserInSession()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public static ApiUser Usuario
        {
            get 
            {
                var identity = (ClaimsIdentity)Thread.CurrentPrincipal?.Identity;
                var userData = identity.FindFirst(ClaimTypes.UserData)?.Value;
                return JsonConvert.DeserializeObject<ApiUser>(userData);
            }
        }
    }
}