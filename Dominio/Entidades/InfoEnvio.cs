using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dominio.Entidades
{
    public class InfoEnvio
    {
        [Required(ErrorMessage = "Porfavor introduce un nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Porfavor introduce un telefono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Porfavor introduce una direccion")]
        [Display(Name = "Direccion Principal")]
        public string Direccion1 { get; set; }

        [Display(Name = "Direccion Secundaria")]
        public string Direccion2 { get; set; }

        [Required(ErrorMessage = "Porfavor introduce una ciudad")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Porfavor introduce una provincia")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "Porfavor introduce un codigo postal")]
        [MinLength(5, ErrorMessage = "El codigo postal tiene que tener 5 numeros")]
        [Display(Name = "Codigo Postal")]
        public string CodigoPostal { get; set; }

        public string Email { get; set; }
    }
}
