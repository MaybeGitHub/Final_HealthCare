using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Dominio.Entidades
{
    public class ProcesadorPedidos
    {
        private configuracionEmail configEmail = new configuracionEmail();

        public class configuracionEmail
        {
            public string Remitente = "proyectos.clase.net@gmail.com";
            public bool SSL = true;
            public string User = "proyectos.clase.net@gmail.com";
            public string Pass = "solosequenosenada";
            public string Host = "smtp.gmail.com";
            public int Puerto = 587;
        }

        public void terminarPedido(Carro carro, Usuario user)
        {
            using (var clienteSMTP = new SmtpClient())
            {
                clienteSMTP.EnableSsl = configEmail.SSL;
                clienteSMTP.Host = configEmail.Host;
                clienteSMTP.Port = configEmail.Puerto;
                clienteSMTP.UseDefaultCredentials = false;
                clienteSMTP.Credentials = new NetworkCredential(configEmail.User, configEmail.Pass);
                MailMessage mensaje = new MailMessage(configEmail.Remitente, user.Email, "Compra realizada en HealthCare", bodyHTML(carro));
                mensaje.IsBodyHtml = true;
                clienteSMTP.Send(mensaje);
            }
        }

        public string bodyHTML(Carro carro)
        {
            string html = "<!DOCTYPE html><html>"
                + "<h3>Mostrar Compra</h3>"
                + "<p>Gracias por utilizar nuestra página. A continuacion le mostramos el contenido de su compra</p>"
                + "<table id = 'tablaCarro'>"
                + "<thead>"
                    + "<tr>"
                        + "<th>Cantidad</th>"
                        + "<th>Producto</th>"
                        + "<th>Empresa</th>"
                        + "<th>Precio</th>"
                    + "</tr>"
                + "</thead>"
                + "<tbody>";
                   
            foreach (ProductoComprado prod in carro.ProductosComprados)
            {
                html += "<tr>"
                            + "<td>" + prod.Cantidad + "</td>"
                            + "<td>" + prod.producto.Nombre + "</td>"
                            + "<td>" + prod.empresa.Nombre + "</td>"
                            + "<td>" + prod.producto.Precio.ToString("c") + "</td>"
                      + "</tr>";
            }
            html += "</tbody>"
                   + "<tfoot>"
                        + "<tr>"
                            + "<th style='float:right' colspan='4'>Total : " + carro.CosteTotalCarro().ToString("c") + "</th>"
                        + "</tr>"
                  + "</tfoot>"
                  + "</table>"               
                  + "</body></html>";



             

            return html;

        }
    }
}
