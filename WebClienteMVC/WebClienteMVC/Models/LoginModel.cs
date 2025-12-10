using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClienteMVC.Models
{
    public class LoginModel
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Error { get; set; }
    }
}