using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Helpers;
using System.Security.Claims;
using FrontEnd.Seguridad;

[assembly: OwinStartup("StartupConfiguration", typeof(FrontEnd.Startup))]
namespace FrontEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName = "SPL",
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                LogoutPath = new PathString("/Account/Login"),
                SlidingExpiration = true
            });

            app.Use<SessionMiddleware>();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
