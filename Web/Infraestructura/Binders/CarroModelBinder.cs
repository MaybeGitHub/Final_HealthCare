using Dominio.Entidades;
using System.Web.Mvc;

namespace Web.Infraestructura.Binders
{
    public class CarroModelBinder : IModelBinder
    {
        private const string sessionKey = "Carro";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Carro carro = null;

            if ( controllerContext.HttpContext.Session != null)
            {
                carro = (Carro)controllerContext.HttpContext.Session[sessionKey];
            }

            if( carro == null)
            {
                carro = new Carro();
                if( controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = carro;
                }
            }

            return carro;
        }
    }
}