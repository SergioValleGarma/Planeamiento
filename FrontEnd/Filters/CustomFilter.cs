using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FrontEnd.Filters
{
    public class CustomFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var ctx = HttpContext.Current.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            bool isAuthorized = authenticationManager.User.Identity.IsAuthenticated;

            //bool isAuthorized = IsAuthorized(filterContext); // check authorization
            base.OnAuthorization(filterContext);
            if (!isAuthorized && !filterContext.ActionDescriptor.ActionName.Equals("Unauthorized", StringComparison.InvariantCultureIgnoreCase)
                && !filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("LogOn", StringComparison.InvariantCultureIgnoreCase))
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
        }
    }
}