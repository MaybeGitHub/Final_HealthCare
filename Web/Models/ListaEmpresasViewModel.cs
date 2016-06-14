using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ListaEmpresasViewModel
    {
        public IEnumerable<Usuario> Empresas { get; set; }
        public InformacionPagina InfoPagina { get; set; }

        public Producto Producto { get; set; }
    }
}