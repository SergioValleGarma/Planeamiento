using Common.Auth;
using Common.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FrontEnd.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Primero, verifica la autorización básica (si el usuario está autenticado)
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            // Obtiene el usuario actual
            var user = httpContext.User as ClaimsPrincipal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            // Obtiene el claim UserData que contiene la información de permisos
            var userDataClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
            if (userDataClaim == null)
            {
                return false;
            }

            if ((ControllerName == null && ActionName == null) || ControllerName == "Account")
            {
                return true;
            }

            // Deserializa el UserData para obtener el objeto User
            var userData = JsonConvert.DeserializeObject<ApiUser>(userDataClaim.Value);

            // Construye el acceso requerido en el formato "/Controller/Action"
            var requiredAccess = $"{ControllerName}/{ActionName}";

            // Verifica si el usuario tiene el acceso requerido
            return userData.opciones.Any(opcion => opcion.vURL.Equals(requiredAccess, StringComparison.OrdinalIgnoreCase));
        }

        // Maneja la solicitud no autorizada redirigiendo a una acción de "Unauthorized"
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Account", action = "Login" }));
        }
    }
}