using System;
namespace Web.Models
{
    public class InformacionPagina
    {
        public int ProductosTotales { get; set; }
        public int ProductosPagina { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas {
            get {
                return (int)Math.Ceiling((decimal)ProductosTotales / ProductosPagina);
            }
        }
    }
}