using System.Collections.Generic;
using Dominio.Entidades;

namespace Web.Models
{
    public class ListaProductosViewModel
    {
        public IEnumerable<Producto> Productos { get; set; }
        public InformacionPagina InfoPagina { get; set; }
        public string CategoriaActual { get; set; }
    }
}