using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClienteMVC.WCF_Apl_Dis;

namespace WebClienteMVC.Controllers
{
    public class VentasController : Controller
    {
        Service1Client servicio = new Service1Client();

        [HttpPost]
        public ActionResult RegistrarVenta(string json)
        {
            try
            {
                // Convertir JSON del carrito a lista de C# 
                var detalles = Newtonsoft.Json.JsonConvert
                    .DeserializeObject<List<Cls_DetalleVenta>>(json);

                Cls_Ventas venta = new Cls_Ventas
                {
                    Id_Trabajador = null,
                    Id_Cliente = Convert.ToInt32(Session["IdUsuario"]),
                    Fecha_Pedido = DateTime.Now,
                    Metodo_Pago = "TARJETA",
                    Estado = "Pendiente",
                    DetalleVenta = detalles.ToArray(),
                    Monto_Total = Monto_Total
                };

                // 💥 Aquí se envía EXPLÍCITAMENTE como tu test
                var cliente = new Service1Client();
                int idVenta = cliente.Insert_Venta_Return_Id(venta);


                Session["IdVentaActual"] = idVenta;
                return Json(new { ok = true, idVenta = idVenta });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }
        public JsonResult EstadoVentaJson(int id)
        {
            var cliente = new Service1Client();
            var venta = cliente.Get_Estado_Venta(id);

            if (venta == null)
                return Json(new { ok = false }, JsonRequestBehavior.AllowGet);

            // Calcular hora estimada (ejemplo 20 min)
            string horaLista = venta.Fecha_Pedido.AddMinutes(20).ToString("hh:mm tt");

            return Json(new
            {
                ok = true,
                estado = venta.Estado,
                horaLista = horaLista
            }, JsonRequestBehavior.AllowGet);
        }
    }
}