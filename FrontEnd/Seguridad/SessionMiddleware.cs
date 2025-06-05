using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FrontEnd.Seguridad
{
    public class SessionMiddleware : OwinMiddleware
    {
        private string AuthenticationUrl = ConfigurationManager.AppSettings.Get("LoginEndPoint");
        private string iCodAplicacion = ConfigurationManager.AppSettings.Get("iCodAplicacion");
        public SessionMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            var authManager = context.Authentication;
            var authCookie = context.Request.Cookies["SPL"];
            var currentPath = context.Request.Path.ToString().ToLower();

            // Verificar si la solicitud es para el Login
            if (currentPath == "/account/login" || currentPath == "/account/connect")
            {
                // Si la solicitud es para login, no necesitamos verificar la autenticación
                await Next.Invoke(context);
                return;
            }

            if (!string.IsNullOrEmpty(authCookie))
            {
                var user = context.Authentication.User;

                // Verificar si el usuario está autenticado
                if (user != null && user.Identity.IsAuthenticated)
                {
                    // Si la sesión es válida, continuamos con la solicitud
                    await Next.Invoke(context);
                }
                else
                {
                    // Si la cookie ha expirado o no está presente, cerramos la sesión y redirigimos a login
                    authManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                    context.Response.Redirect(AuthenticationUrl + $"?id=" + iCodAplicacion);
                }
            }
            else
            {
                // Si no hay cookie, redirigimos a login
                context.Response.Redirect(AuthenticationUrl + $"?id=" + iCodAplicacion);
            }
        }
    }

}