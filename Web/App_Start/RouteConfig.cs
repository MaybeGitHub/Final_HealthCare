using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");            

            routes.MapRoute(                
                "inicio",
                "",
                new { controller = "Home", action = "Index" }
            );

            routes.MapRoute("login", "Login/Index", new { controller = "Login", action = "Index" });

            routes.MapRoute(
                "Pedidos",
                "{controller}/{action}/{estado}",
                new { controller = "Empresas", action = "Pedidos", opcion = "{estado}" },
                new { opcion = @"^[0-2]$" }
            );

            routes.MapRoute(
                "Pagina",
                "Pagina{pagina}",
                new { controller = "Productos", action = "Listar", categoria = (string)null },
                new { pagina = @"\d+" }
            );

            routes.MapRoute(
                "Todo",
                "{categoria}/{pagina}",
                new { controller = "{categoria}", action = "{pagina}" },
                new { pagina = @"\d+" }
            );

            routes.MapRoute("TodoLoDemas", "{controller}/{action}");
        }
    }
}
