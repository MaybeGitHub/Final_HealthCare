using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Usuario es obligatorio")]
        public string User { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string Password { get; set; }
    }
}