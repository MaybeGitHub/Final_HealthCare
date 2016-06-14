using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Web.Models;
using System.Data.Linq;
using System.Drawing;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private BaseDatos baseDatos = new BaseDatos(ConfigurationManager.ConnectionStrings["baseDatos"].ConnectionString);

        public bool checkSession()
        {
            return Session["ID"] == null;
        }

        public ActionResult Index()
        {
            if (checkSession()) return RedirectToRoute("login");
            ViewBag.Empresa = baseDatos.Usuarios.Where(u => u.Id == (int)Session["ID"]).SingleOrDefault().Nombre;
            return View(baseDatos.Productos.Where(producto => producto.Empresa == (int)Session["ID"]));
        }

        public ActionResult Editar(int ID)
        {
            if (checkSession()) return RedirectToRoute("login");
            Producto producto = baseDatos.Productos.FirstOrDefault(p => p.Id == ID);
            return View(producto);
        }

        public FileContentResult GetImage(int id)
        {
            Producto prod = baseDatos.Productos.FirstOrDefault(p => p.Id == id);
            if (prod != null && prod.imagenContenido != null) {
                return File(prod.imagenContenido, prod.imagenTipo);
            }
            else
            {                
                return File(System.IO.File.ReadAllBytes(Server.MapPath("~/Imagenes/default/sin_foto.png")), "image/png");
            }
        }

        [HttpPost]
        public ActionResult Editar(Producto producto, HttpPostedFileBase imagen = null)
        {

            if (ModelState.IsValid)
            {
                if (imagen != null)
                {
                    producto.imagenTipo = imagen.ContentType;
                    producto.imagenContenido = new byte[imagen.ContentLength];
                    imagen.InputStream.Read(producto.imagenContenido, 0, imagen.ContentLength);
                }

                if (producto.Id == 0)
                {
                    baseDatos.Productos.InsertOnSubmit(producto);
                }
                else
                {                    
                    Producto productoBD = baseDatos.Productos.FirstOrDefault(p => p.Id == producto.Id);
                    productoBD.imagenTipo = producto.imagenTipo;
                    productoBD.imagenContenido = producto.imagenContenido;
                    productoBD.Nombre = producto.Nombre;
                    productoBD.Descripcion = producto.Descripcion;
                    productoBD.Categoria = producto.Categoria;
                    productoBD.Precio = producto.Precio;
                }

                baseDatos.SubmitChanges();

                TempData["Mensaje"] = string.Format("{0} ha sido guardado", producto.Nombre);                
                return RedirectToAction("Index");
            }
            else
            {            
                return View(producto);
            }
        }

        public ActionResult Crear()
        {
            if (checkSession()) return RedirectToRoute("login");
            return View("Editar", new Producto());
        }

        [HttpPost]
        public ActionResult Borrar(int IDProducto)
        {
            Producto productoBorrar = baseDatos.Productos.FirstOrDefault(p => p.Id == IDProducto );

            if (productoBorrar != null) {
                baseDatos.Productos.DeleteOnSubmit(productoBorrar);
                baseDatos.SubmitChanges();
                TempData["Mensaje"] = string.Format("{0} borrado con exito", productoBorrar.Nombre);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Upload()
        {
            if (checkSession()) return RedirectToRoute("login");
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] files, string delimitador)
        {
            List<string> OK = new List<string>();
            List<string> KO = new List<string>();
            if (files[0] != null)
            {
                string filename = "";
                foreach (HttpPostedFileBase file in files)
                {
                    string filepathtosave = "";
                    try
                    {
                        filename = file.FileName;
                        filepathtosave = "~/Imagenes/" + filename;
                        file.SaveAs(Server.MapPath(filepathtosave));

                        string[] contenidoFile = System.IO.File.ReadAllLines(Server.MapPath("~/Imagenes/" + filename));

                        Dictionary<int, string> cabeceras = new Dictionary<int, string>();

                        string[] splitCabeceras = contenidoFile[0].Split(Convert.ToChar(delimitador));
                        int longSplit = splitCabeceras.Length;
                        string cabecera = "";
                        string[] cabecerasInteresantes = new string[] { "nombre", "descripcion", "precio", "categoria" };
                        for (int i = 0; i < longSplit; i++)
                        {
                            cabecera = splitCabeceras[i].ToLower();
                            if (cabecerasInteresantes.Contains(cabecera) && !cabeceras.ContainsValue(cabecera))
                            {
                                cabeceras[i] = cabecera;
                            }
                        }

                        string linea = "";
                        int longitud = contenidoFile.Length;
                        string[] splitLinea;
                        Producto producto;
                        string valor;
                        for (int i = 1; i < longitud; i++)
                        {
                            linea = contenidoFile[i];
                            producto = new Producto();
                            producto.Empresa = (int)Session["ID"];
                            splitLinea = linea.Split(Convert.ToChar(delimitador));
                            for (int j = 0; j < splitLinea.Length; j++)
                            {
                                valor = splitLinea[j];
                                if (cabeceras.ContainsKey(j))
                                {
                                    switch (cabeceras[j])
                                    {
                                        case "nombre":
                                            producto.Nombre = valor;
                                            break;
                                        case "descripcion":
                                            producto.Descripcion = valor;
                                            break;
                                        case "categoria":
                                            producto.Categoria = valor;
                                            break;
                                        case "precio":
                                            valor = valor.Replace(".", ",");
                                            producto.Precio = Convert.ToDecimal(valor);
                                            break;
                                    }
                                }
                            }
                            if (baseDatos.Productos.Where(p => p.Empresa == producto.Empresa && p.Nombre == producto.Nombre).SingleOrDefault() == null)
                            {
                                baseDatos.Productos.InsertOnSubmit(producto);
                            }
                        }
                        
                        OK.Add("Fichero " + file.FileName + " Subido correctamente");
                    }
                    catch
                    {
                        KO.Add("Error al subir el fichero " + file.FileName);
                    }

                    baseDatos.SubmitChanges();
                    System.IO.File.Delete(Server.MapPath(filepathtosave));
                }
            }
            ViewBag.OK = OK;
            ViewBag.KO = KO;            
            return View();
        }
    }
}