using Dominio.Entidades;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Infraestructura.Binders;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(Carro), new CarroModelBinder());
        }
    }
}
