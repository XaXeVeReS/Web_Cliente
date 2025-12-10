using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebClienteMVC.Datos;
using WebClienteMVC.Models;
using WebClienteMVC.WCF_Apl_Dis;

namespace WebClienteMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly PlatosDatos datos = new PlatosDatos();
        Service1Client cliente = new Service1Client();
        public async Task<ActionResult> Platos()
        {
            var platos = await datos.ListarPlatosAsync();
            return View(platos);
        }

        public ActionResult DetallePlato(int id)
        {
            

            // 1. OBTENER PLATO COMPLETO
            var plato = cliente.Search_PlatoAsync(id).Result;

            // 2. OBTENER STOCK DISPONIBLE
            var stock = cliente.Check_StockAsync(id).Result;

            ViewBag.Stock = stock;

            return PartialView("_DetallePlato", plato);
        }
//-------------------------------------------------------------------//
        public async Task<ActionResult> Promociones()
        {
            var promociones = await cliente.Get_Promocion_PlatosAsync();

            var lista = promociones
                .GroupBy(x => x.Id_Promocion)
                .Select(g => g.First())
                .ToList();

            return View(lista);
        }

        public async Task<ActionResult> DetallePromocion(int idPromo)
        {
            
            var lista = await cliente.Get_Promocion_PlatosAsync();

            // Filtrar por la promo seleccionada
            var promo = lista.Where(x => x.Id_Promocion == idPromo).ToList();

            if (!promo.Any())
                return HttpNotFound("Promoción no encontrada");

            // Obtener stock del PRIMER plato
            int idPlato = promo.First().Id_Plato;
            var stock = await cliente.Check_StockAsync(idPlato);

            ViewBag.Stock = stock;

            return PartialView("_DetallePromocionModal", promo);
        }

//-------------------------------------------------------------------------------------//
        public ActionResult Estado()
        {
            return View();
        }


        public JsonResult EstadoVentaJson(int id)
        {
            var cliente = new Service1Client();
            var venta = cliente.Get_Estado_Venta(id);

            if (venta == null)
                return Json(new { ok = false }, JsonRequestBehavior.AllowGet);

            return Json(new { ok = true, estado = venta.Estado }, JsonRequestBehavior.AllowGet);
        }
    }
}