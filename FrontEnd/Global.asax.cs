using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

namespace FrontEnd
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new UnityContainer();

            // Registrar servicios
            container.RegisterType<AccionEstrategicaService>();
            container.RegisterType<ActividadOperativaActualizadaService>();
            container.RegisterType<ActividadOperativaService>();
            container.RegisterType<BloqueoService>();
            container.RegisterType<CatalogoActividadOperativaService>();
            container.RegisterType<DependenciaService>();
            container.RegisterType<ObjetivoEstrategicoService>();
            container.RegisterType<ProgramacionFinancieraService>();
            container.RegisterType<SeguimientoTareaService>();
            container.RegisterType<TareaActualizadaService>();
            container.RegisterType<TareaService>();
            container.RegisterType<DatabaseService>();

            // Configurar el resolver de dependencias
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            MvcHandler.DisableMvcResponseHeader = true;
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{

        //    Exception exception = Server.GetLastError();
        //    Response.Clear();

        //    HttpException httpException = exception as HttpException;

        //    int error = httpException != null ? httpException.GetHttpCode() : 0;

        //    Uri uri = Request.Url;
        //    string codeError = DateTime.Now.ToString("HHmmssfffffff");

        //    Server.ClearError();
        //    Response.Redirect(String.Format("~/Error/?error={0}", "", exception.Message));
        //}
    }
}
