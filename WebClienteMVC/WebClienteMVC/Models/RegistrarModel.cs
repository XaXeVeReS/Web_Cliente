using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClienteMVC.Models
{
    public class RegistrarModel
    {
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingrese un usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Ingrese un email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ingrese un teléfono")]
        public string Telefono { get; set; }

        public string Rol { get; set; } = "Cliente";

        [Required(ErrorMessage = "Ingrese su contraseña")]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d).{9,}$",
        ErrorMessage = "La contraseña debe tener al menos 9 caracteres, una mayúscula y un número")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirme su contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; }

        public string Error { get; set; }

    }
}