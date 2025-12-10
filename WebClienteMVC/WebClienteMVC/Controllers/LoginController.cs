using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using WebClienteMVC.Models;

namespace WebClienteMVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UsuarioDatos datos = new UsuarioDatos();

        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                int IdUsuario = await datos.LoginAsync(model.Usuario, model.Clave);

                if (IdUsuario > 0)
                {
                    Session["IdUsuario"] = IdUsuario;
                    Session["Usuario"] = model.Usuario;
                   
                    FormsAuthentication.SetAuthCookie(model.Usuario, false);
                    return RedirectToAction("Platos", "Home");
                }

                model.Error = "Usuario o contraseña incorrectos.";
                return View(model);
            }
            catch (Exception ex)
            {
                model.Error = ex.Message;
                return View(model);
            }
        }
        public ActionResult Salir()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
