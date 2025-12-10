using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClienteMVC.Models
{
    public class EstadoPedidoModel
    {
        public int Id_Venta { get; set; }
        public string Estado { get; set; }   
        public string Hora_Estimada { get; set; }
        public bool ComentarioRealizado { get; set; }
    }
}