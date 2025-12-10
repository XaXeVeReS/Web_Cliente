using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClienteMVC.Models
{
    public class CambiarPasswordModel
    {
        [Required(ErrorMessage = "Ingrese su email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ingrese el token enviado a su correo")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Ingrese una nueva contraseña")]
        public string NuevaClave { get; set; }

        [Required(ErrorMessage = "Confirme su nueva contraseña")]
        [Compare("NuevaClave", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarClave { get; set; }
    }
}