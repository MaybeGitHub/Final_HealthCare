using System.Linq;
using System.Web.Mvc;
using Dominio.Entidades;
using System.Configuration;
using Web.Models;
using System.Web.Security;
using System.Collections.Generic;
using System;

namespace Web.Controllers
{
    public class ProductosController : Controller
    {
        private BaseDatos baseDatos = new BaseDatos(ConfigurationManager.ConnectionStrings["baseDatos"].ConnectionString);
        int MaxProductosPagina = 6;
        int MaxEmpresasPagina = 6;

        public bool checkSession()
        {
            return Session["ID"] == null;
        }

        public ActionResult Listar(string categoria, int pagina = 1)
        {         
            if (checkSession()) return RedirectToRoute("login");

            IEnumerable<Producto> productos = baseDatos.Productos
                    .Where(p => categoria == null || p.Categoria == categoria)
                    .Skip((pagina - 1) * MaxProductosPagina)
                    .Take(MaxProductosPagina);

            List<Producto> productosFiltrados = new List<Producto>();

            foreach(Producto prod in productos)
            {
                bool encontrado = false;
                foreach(Producto p in productosFiltrados)
                {
                    if(p.Nombre == prod.Nombre)
                    {
                        encontrado = true;
                    }                   
                }

                if (!encontrado)
                {
                    productosFiltrados.Add(prod);
                }
            }

            ListaProductosViewModel modelo = new ListaProductosViewModel
            {
                Productos = productosFiltrados.AsEnumerable(),

                InfoPagina = new InformacionPagina
                {
                    PaginaActual = pagina,
                    ProductosPagina = MaxProductosPagina,
                    ProductosTotales = categoria == null ? baseDatos.Productos.Count() : baseDatos.Productos.Where(p => p.Categoria == categoria).Count()
                },

                CategoriaActual = categoria
            };

            return View(modelo);
        }        

        public ActionResult SeleccionarEmpresa( string nombre, string UrlAnterior, int pagina = 1 )
        {
            if (checkSession()) return RedirectToRoute("login");

            Producto producto = baseDatos.Productos.FirstOrDefault(p => p.Nombre == nombre);

            ListaEmpresasViewModel modelo = new ListaEmpresasViewModel
            {
                Empresas = baseDatos.Usuarios
                    .Where(empresa => empresa.Productos.Where(p => p.Nombre == nombre).Count() > 0 )
                    .OrderBy(empresa => empresa.Id)
                    .Skip((pagina - 1) * MaxEmpresasPagina)
                    .Take(MaxEmpresasPagina),

                InfoPagina = new InformacionPagina
                {
                    PaginaActual = pagina,
                    ProductosPagina = MaxEmpresasPagina,
                    ProductosTotales = baseDatos.Usuarios.Select(e => e.Productos).Count()
                },

                Producto = producto
            };

            ViewBag.UrlAnterior = UrlAnterior;
            return View(modelo);
        }

        public ActionResult Pedidos(string año1, string año2, string mes1, string mes2, string estado = "activas" )
        {
            if (checkSession()) return RedirectToRoute("login");
            ViewBag.Busqueda = "Activas";
            IEnumerable<Pedido> pedidos = baseDatos.Pedidos.Where(pedido => pedido.Usuario == (int)Session["ID"]).OrderByDescending(p => p.Fecha_Insercion);
            switch (estado)
            {
                case "activas":
                    pedidos = pedidos.Where(p => p.Estado < 2);
                    ViewBag.Busqueda = "Activas";
                    break;
                case "espera":
                    pedidos = pedidos.Where(p => p.Estado == 0);
                    ViewBag.Busqueda = "En Espera";
                    break;
                case "curso":
                    pedidos = pedidos.Where(p => p.Estado == 1);
                    ViewBag.Busqueda = "En Curso";
                    break;
                case "terminadas":
                    pedidos = pedidos.Where(p => p.Estado == 2);
                    ViewBag.Busqueda = "Terminadas";
                    break;
                case "rechazadas":
                    pedidos = pedidos.Where(p => p.Estado == 3);
                    ViewBag.Busqueda = "Rechazadas";
                    break;
                default:
                    ViewBag.Busqueda = "Todas";
                    break;
            }

            DateTime fecha1;
            DateTime fecha2;

            if (año1 != null && año1 != "")
            {
                fecha1 = new DateTime(Convert.ToInt32(año1), 1, 1);
                if (año2 != null && año2 != "")
                {
                    fecha2 = new DateTime(Convert.ToInt32(año1), 1, 1);
                    pedidos = pedidos.Where(p => p.Fecha_Insercion > fecha1 && p.Fecha_Insercion < fecha2);
                }
                else
                {
                    pedidos = pedidos.Where(p => p.Fecha_Insercion > fecha1 && p.Fecha_Insercion < DateTime.Now);
                }
            }
            else
            {
                año1 = 2000.ToString();
            }

            if (mes1 != null && mes1 != "")
            {
                fecha1 = new DateTime(Convert.ToInt32(año1), Convert.ToInt32(mes1), 1);
                if (mes2 != null && mes2 != "")
                {
                    fecha2 = new DateTime(Convert.ToInt32(año1), Convert.ToInt32(mes2), 1);
                    pedidos = pedidos.Where(p => p.Fecha_Insercion > fecha1 && p.Fecha_Insercion < fecha2);
                }
                else
                {
                    pedidos = pedidos.Where(p => p.Fecha_Insercion > fecha1 && p.Fecha_Insercion < DateTime.Now);
                }
            }

            return View(pedidos);
        }

        public string Accion(int id, int accion)
        {
            Pedido pedido = baseDatos.Pedidos.Where(p => p.Id == id).SingleOrDefault();

            switch (accion)
            {
                case 0:
                    pedido.Estado += 1;
                    if (pedido.Estado == 2)
                    {
                        pedido.Fecha_Entregado = DateTime.Now;
                    }
                    break;
                case 1:
                    pedido.Estado = 3;
                    break;
                case 2:
                    baseDatos.Pedidos.DeleteOnSubmit(pedido);
                    break;
            }

            baseDatos.SubmitChanges();
            return "ok";
        }
    }
}