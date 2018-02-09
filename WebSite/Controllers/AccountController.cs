using System.Web.Mvc;
using System.Web.Security;
using Zidium.Examples.Models;

namespace Zidium.Examples.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {        
                if (model.Password == "1")
                {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");                    
                }
                else
                {
                    ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
                }
            }
            return View(model);
        }
        
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}