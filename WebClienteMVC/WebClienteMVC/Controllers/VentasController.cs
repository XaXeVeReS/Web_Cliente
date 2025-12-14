using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebClienteMVC.WCF_Apl_Dis;

namespace WebClienteMVC.Controllers
{
    public class VentasController : Controller
    {
        // Instancia única del servicio para reutilizar
        Service1Client servicio = new Service1Client();

        [HttpPost]
        public ActionResult RegistrarVenta(string json, string banco, string tarjeta, string fecha, string cvv)
        {
            try
            {
                // 1. VALIDACIÓN DE SESIÓN (Evita pantalla amarilla si la sesión expira)
                if (Session["IdUsuario"] == null)
                {
                    return Json(new { ok = false, error = "Tu sesión ha expirado. Por favor inicia sesión de nuevo." });
                }

                int idCliente = (int)Session["IdUsuario"];

                // 2. BLOQUEO DE SEGURIDAD
                int idVentaExistente = servicio.Search_id_venta_activa(idCliente);

                if (idVentaExistente > 0)
                {
                    // CORRECCIÓN: No uses RedirectToAction aquí. Envía la URL en el JSON.
                    return Json(new
                    {
                        ok = false,
                        isRedirect = true, // Bandera para que JS sepa qué hacer
                        redirectUrl = Url.Action("Estado", "Home"),
                        error = "Ya tienes un pedido en curso."
                    });
                }

                // 3. DESERIALIZAR Y CALCULAR
                var detalles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cls_DetalleVenta>>(json);
                float totalPagar = 0;

                foreach (var item in detalles)
                {
                    // Lógica de precios (Correcta)
                    if (item.Id_Promocion != null && item.Id_Promocion > 0)
                    {
                        totalPagar += item.Precio_Unitario * item.Cantidad;
                    }
                    else
                    {
                        float desc = item.Descuento ?? 0;
                        totalPagar += (item.Precio_Unitario * item.Cantidad) * (1 - desc);
                    }
                }

                // 4. PROCESAR PAGO
                Cls_Cartera objCartera = new Cls_Cartera
                {
                    Tipo = banco,
                    Nro_Tarjeta = tarjeta,
                    FechaVencimiento = fecha,
                    Clave_Pin = cvv
                };

                string mensajePago;
                bool pagoExitoso = servicio.Procesar_Pago(objCartera, totalPagar, out mensajePago);

                if (!pagoExitoso)
                {
                    return Json(new { ok = false, error = mensajePago });
                }

                // 5. GUARDAR VENTA
                Cls_Ventas venta = new Cls_Ventas
                {
                    Id_Cliente = idCliente,
                    Fecha_Pedido = DateTime.Now,
                    Metodo_Pago = "Tarjeta",
                    Estado = "Pendiente",
                    Costo_Total = totalPagar,
                    Monto_Total = totalPagar,
                    DetalleVenta = detalles.ToArray()
                };

                int idVenta = servicio.Insert_Venta_Return_Id(venta);

                return Json(new { ok = true, idVenta = idVenta });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = "Error en el servidor: " + ex.Message });
            }
        }

        public JsonResult EstadoVentaJson(int id)
        {
            // Reutilizamos la variable 'servicio' de arriba
            var venta = servicio.Get_Estado_Venta(id);

            if (venta == null)
                return Json(new { ok = false }, JsonRequestBehavior.AllowGet);

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