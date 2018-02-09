using System;
using System.Web.Mvc;
using Zidium.Api;

namespace Zidium.Examples.Controllers
{
    /// <summary>
    /// Базовый класс всех контроллеров
    /// </summary>
    public abstract class ControllerBase : Controller
    {        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);            

            if (Request.IsAuthenticated)
            {
                var user = HttpContext.User.Identity.Name;
                var ip = Request.UserHostAddress;

                var message = string.Format(
                    "Пользователь {0} c IP {1}",
                    user,
                    ip);

                var component = Client.Instance.GetDefaultComponentControl();

                component
                    .CreateComponentEvent("Пользователь на сайте", message)
                    .SetImportance(EventImportance.Success)
                    .SetJoinInterval(TimeSpan.FromMinutes(10))
                    .SetJoinKey(user, ip)                   
                    .Add();
            }
        }
    }
}