using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private  BaseDatos baseDatos = new BaseDatos(ConfigurationManager.ConnectionStrings["baseDatos"].ConnectionString);

        public ViewResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario user = baseDatos.Usuarios.FirstOrDefault(u => u.Login == usuario.User && u.Password == usuario.Password);
                if ( user != null)
                {                    
                    Session["ID"] = user.Id;
                    switch (user.Rol_Info.Tipo)
                    {
                        case "Cliente": return RedirectToAction("Listar", "Productos");
                        case "Empresa": return RedirectToAction("Index", "Empresas");
                        default: return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "El usuario no existe");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public RedirectToRouteResult Desconectar()
        {
            Session.Remove("ID");
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Cuenta()
        {
            string previousPage = Request.UrlReferrer.AbsolutePath;

            if (previousPage != "/Login/Index" && previousPage != "/Login/Direccion" && previousPage != "/")
            {
                return RedirectToRoute("inicio");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Cuenta(Usuario usuario, HttpPostedFileBase foto = null)
        {
            if (ModelState.IsValid)
            {
                if(baseDatos.Usuarios.FirstOrDefault(u => u.Login == usuario.Login) != null)
                {
                    ModelState.AddModelError("Login", "El login ya existe");
                    return View();
                }
                if (foto != null)
                {
                    usuario.imagenTipo = foto.ContentType;
                    usuario.imagenContenido = new byte[foto.ContentLength];
                    foto.InputStream.Read(usuario.imagenContenido, 0, foto.ContentLength);
                }

                if (usuario.Id == 0)
                {
                    baseDatos.Usuarios.InsertOnSubmit(usuario);
                }
                else
                {
                    Usuario usu = baseDatos.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
                    usu = usuario;
                }

                baseDatos.SubmitChanges();

                int user = baseDatos.Usuarios.Where(u => u.Login == usuario.Login && u.Password == usuario.Password).Select(u => u.Id).SingleOrDefault();
                Session["ID"] = usuario.Id;
                return RedirectToAction("Direccion");

            }
            else
            {
                return View();
            }
        }

        public ActionResult Direccion()
        {
            string previousPage = Request.UrlReferrer.AbsolutePath;

            if (previousPage != "/Login/Cuenta")
            {
                return RedirectToRoute("inicio");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Direccion(Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                if (direccion.Id == 0)
                {                    
                    baseDatos.Direccions.InsertOnSubmit(direccion);
                    baseDatos.SubmitChanges();
                }                
                Usuario user = baseDatos.Usuarios.FirstOrDefault(d => d.Id == (int)Session["ID"]);
                user.Direccions = direccion;
                baseDatos.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
            
        public ActionResult EditarCuenta()
        {
            Usuario user = baseDatos.Usuarios.FirstOrDefault(u => u.Id == (int)Session["ID"]);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditarCuenta(Usuario usuario, HttpPostedFileBase foto = null)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    usuario.imagenTipo = foto.ContentType;
                    usuario.imagenContenido = new byte[foto.ContentLength];
                    foto.InputStream.Read(usuario.imagenContenido, 0, foto.ContentLength);
                }
                
                Usuario usu = baseDatos.Usuarios.FirstOrDefault(u => u.Id == (int)Session["ID"]);
                usu.Nombre = usuario.Nombre;
                usu.Apellidos = usuario.Apellidos;
                usu.imagenContenido = usuario.imagenContenido;
                usu.imagenTipo = usuario.imagenTipo;
                usu.Login = usuario.Login;
                usu.Password = usuario.Password;
                usu.Telefono = usuario.Telefono;
                usu.Email = usuario.Email;
                baseDatos.SubmitChanges();
                return RedirectToAction("Listar", "Productos");
            }
            else
            {
                return View();
            }
        }

        public ActionResult EditarDireccion()
        {
            return View(baseDatos.Usuarios.FirstOrDefault(u => u.Id == (int)Session["ID"]).Direccions);
        }

        [HttpPost]
        public ActionResult EditarDireccion(Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                Usuario user = baseDatos.Usuarios.FirstOrDefault(d => d.Id == (int)Session["ID"]);
                Direccion dir = baseDatos.Direccions.FirstOrDefault(d => d.Id == user.Direccions.Id);
                dir.Calle = direccion.Calle;
                dir.CodigoPostal = direccion.CodigoPostal;
                dir.Localidad = direccion.Localidad;
                dir.Provincia = direccion.Provincia;
                baseDatos.SubmitChanges();
                return RedirectToAction("Listar", "Productos");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Recuperar()
        {
            return View();
        }

        public JsonResult getUsuario(string login)
        {
            Usuario user = baseDatos.Usuarios.FirstOrDefault(u => u.Login == login);        
            if(user != null)
            {
                return Json(new { telefono = user.Telefono, nombre = user.Nombre, apellidos= user.Apellidos, contraseña=user.Password });
            }
            else
            {
                return null;
            }
            
        }        
    }
}