using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dominio.Entidades;
using System.Configuration;

namespace Web.Controllers
{
    public class NavegadorController : Controller
    {
        private BaseDatos baseDatos = new BaseDatos(ConfigurationManager.ConnectionStrings["baseDatos"].ConnectionString);

        public PartialViewResult Menu(string categoria = null)
        {

            ViewBag.categoriaSeleccionada = categoria;
            IEnumerable<string> modelo = baseDatos.Productos
                                    .Select(p => p.Categoria)
                                    .Distinct()
                                    .OrderBy(p => p);
            return PartialView(modelo);
        }
    }
}