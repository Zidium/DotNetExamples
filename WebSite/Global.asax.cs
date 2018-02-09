using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;
using Zidium.Api;
using Zidium.Examples.Controllers;
using Zidium.Examples.Helpers;
using Zidium.Examples.Models;

namespace Zidium.Examples
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                // подключим заполнение свойств всех событий-ошибок
                Client.Instance.EventPreparer = new EventPreparer();

                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                // сообщает, что запуск приложения успешно выполнен
                LogManager.GetCurrentClassLogger().Info("Приложение запущено");
            }
            catch (Exception exception)
            {
                // отправим ошибку запуска приложения в мониторинг
                LogManager.GetCurrentClassLogger().Fatal(exception);

                throw; // приложение не должно работать, если есть ошибка инициализации
            }
        }

        private void ShowErrorPage(string viewName, Exception exception, string errorCode = null)
        {
            using (var controller = new HomeController())
            {
                var controllerContext = new ControllerContext(HttpContext.Current.Request.RequestContext, controller);
                var model = new ErrorModel()
                {
                    Exception = exception,
                    ErrorCode = errorCode
                };
                var result = new ViewResult
                {
                    ViewName = "~/Views/Errors/" + viewName,
                    ViewData = { Model = model }
                };
                result.ExecuteResult(controllerContext);
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            if (exception == null)
                return;

            // обработка ошибки 404 - страница не найдена
            // источником ошибок 404 часто бывают боты, которые что то ищут на сайте
            // такие ошибки не будем считать ошибками нашего приложения
            var httpException = exception as HttpException;
            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    ShowErrorPage("Error404.cshtml", exception);
                    return;
                }
            }

            // остальные ошибки залогируем
            LogManager.GetCurrentClassLogger().Error(exception);

            // получим уникальный номер ошибки в Zidium, чтобы показать его пользователю
            var errorNumber = Client.Instance.ExceptionRender.GetExceptionTypeCode(exception);

            // покажем страницу с текстом ошибки и номером
            ShowErrorPage("Error.cshtml", exception, errorNumber);
        }
    }
}
