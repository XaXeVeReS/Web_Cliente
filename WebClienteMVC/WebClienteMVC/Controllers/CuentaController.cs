using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebClienteMVC.WCF_Apl_Dis;
using WebClienteMVC.Models;

namespace WebClienteMVC.Controllers
{
    [AllowAnonymous]
    public class CuentaController : Controller
    {
        private readonly UsuarioDatos datos = new UsuarioDatos();

        // REGISTRAR
        public ActionResult Registrar()
        {
            return View(new RegistrarModel());
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(RegistrarModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var u = new Cls_Usuarios
            {
                Nombre = model.Nombre,
                Rol = model.Rol,
                Email = model.Email,
                Telefono = model.Telefono,
                Password = model.Password
            };

            string error = await datos.RegistrarAsync(u);

            if (error != null)
            {
                model.Error = error;   // ✔ AHORA EL MODAL FUNCIONARÁ
                return View(model);
            }
            return RedirectToAction("Login", "Login");
        }

        // RECUPERAR
        public ActionResult Recuperar() => View(new RecuperarModel());

        [HttpPost]
        public async Task<ActionResult> Recuperar(RecuperarModel model)
        {
            string error = await datos.EnviarTokenAsync(model.Email, model.Usuario);

            if (error != null)
                return RedirectToAction("ModalError", "Util", new { mensaje = error });

            TempData["Email"] = model.Email;
            return RedirectToAction("CambiarPassword");
        }

        // CAMBIAR PASSWORD
        public ActionResult CambiarPassword() => View(new CambiarPasswordModel());

        [HttpPost]
        public async Task<ActionResult> CambiarPassword(CambiarPasswordModel model)
        {
            string error = await datos.CambiarPasswordAsync(model.Email, model.Token, model.NuevaClave);

            if (error != null)
                return RedirectToAction("ModalError", "Util", new { mensaje = error });

            return RedirectToAction("Login", "Login");
        }
    }

}