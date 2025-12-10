using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClienteMVC.Controllers
{
 
    public class UtilController : Controller
    {
        // GET: Util
        [AllowAnonymous]
        public ActionResult ModalError(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            return View("_ModalError");
        }
    }
}