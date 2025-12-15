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
                model.Error = error;
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
            {
                // CAMBIO: Devolvemos la vista con el error para que salga la alerta roja
                model.Error = error;
                return View(model);
            }

            TempData["Email"] = model.Email;
            return RedirectToAction("CambiarPassword");
        }

        // CAMBIAR PASSWORD
        public ActionResult CambiarPassword() => View(new CambiarPasswordModel());
        [HttpPost]
        public async Task<ActionResult> CambiarPassword(CambiarPasswordModel model)
        {
            // 1. VALIDACIÓN DEL MODELO (Aquí se revisa el [Compare] y los [Required])
            if (!ModelState.IsValid)
            {
                // Capturamos el primer error (ej: "Las contraseñas no coinciden")
                // y lo metemos en model.Error para que la Vista lo muestre en el SweetAlert
                string mensajeError = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .FirstOrDefault();

                model.Error = mensajeError;
                return View(model);
            }

            // 2. Si el modelo es válido, procedemos a llamar al WCF
            string error = await datos.CambiarPasswordAsync(model.Email, model.Token, model.NuevaClave);

            if (error != null)
            {
                model.Error = error;
                return View(model);
            }

            return RedirectToAction("Login", "Login");
        }
    }
}