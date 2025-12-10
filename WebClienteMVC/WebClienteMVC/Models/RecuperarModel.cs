using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClienteMVC.Models
{
    public class RecuperarModel
    {
        [Required(ErrorMessage = "Ingrese su contraseña")]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d).{9,}$",
        ErrorMessage = "La contraseña debe tener al menos 9 caracteres, una mayúscula y un número")]
        public string Password { get; set; }
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Ingrese su email")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Email { get; set; }

        public string Error { get; set; }
    }
}