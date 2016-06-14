using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dominio.Entidades;
using System.Configuration;
using Web.Models;
using System;

namespace Web.Controllers
{
    public class CarroController : Controller
    {
        private BaseDatos baseDatos = new BaseDatos(ConfigurationManager.ConnectionStrings["baseDatos"].ConnectionString);
        private ProcesadorPedidos procesadorPedidos = new ProcesadorPedidos();

        public bool checkSession()
        {
            return Session["ID"] == null;
        }

        public ActionResult Index(Carro carro, string UrlAnterior)
        {
            if (checkSession()) return RedirectToRoute("login");
            return View(new CarroViewModel
            {
                Carro = carro,
                UrlAnterior = UrlAnterior

            });
        }

        public RedirectToRouteResult AñadirAlCarro(Carro carro, string nombre, int IDEmpresa, string UrlAnterior)
        {

            Producto producto = baseDatos.Productos.FirstOrDefault(p => p.Nombre == nombre && p.Empresa == IDEmpresa);
            Usuario empresa = baseDatos.Usuarios.FirstOrDefault(e => e.Id == IDEmpresa);
            
            if(producto != null && empresa != null)
            {
                carro.AñadirProducto(producto, empresa, 1);
            }

            return RedirectToAction("Index", new { UrlAnterior });
        }

        public RedirectToRouteResult BorrarDelCarro(Carro carro, int IDProducto, string UrlAnterior)
        {

            Producto producto = baseDatos.Productos.FirstOrDefault(p => p.Id == IDProducto);

            if(producto != null)
            {
                carro.BorrarProducto(producto);
            }

            return RedirectToAction("Index", new { UrlAnterior });
        }

        public PartialViewResult Sumario(Carro carro)
        {

            return PartialView(carro);
        }

        public ActionResult MostrarCompra(Carro carro)
        {
            if (checkSession()) return RedirectToRoute("login");

            if (carro.ProductosComprados.Count() == 0)
            {
                ModelState.AddModelError("", "Lo siento, pero tu carro esta vacio");
                return View("Index", carro);
            }
            return View(carro);
        }

        public ActionResult Terminar(Carro carro)
        {
            if (checkSession()) return RedirectToRoute("login");

            Pedido pedido;
            Usuario user = baseDatos.Usuarios.FirstOrDefault(u => u.Id == (int)Session["ID"]);
            foreach (ProductoComprado prod in carro.ProductosComprados)
            {
                pedido = new Pedido();
                pedido.Empresa = prod.empresa.Id;
                pedido.Estado = 0;
                pedido.Fecha_Insercion = DateTime.Now;
                pedido.Producto = prod.producto.Id;
                pedido.Usuario = user.Id;
                pedido.Cantidad = prod.Cantidad;
                baseDatos.Pedidos.InsertOnSubmit(pedido);
            }
            Session.Remove("Carro");
            baseDatos.SubmitChanges();
            procesadorPedidos.terminarPedido(carro, user);
            carro.VaciarCarro();
            return View("Completado");
        }
    }
}