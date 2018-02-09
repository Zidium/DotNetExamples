using System;
using System.Web.Mvc;
using NLog;

namespace Zidium.Examples.Controllers
{
    public class ErrorsController : ControllerBase
    {
        public ActionResult Sample1()
        {
            throw new Exception("Ой, случилась ошибка (вариант 1)");
        }

        public ActionResult Sample2()
        {
            throw new Exception("Опять ошибка (вариант 2)");
        }

        public ActionResult Sample3()
        {
            try
            {
                throw new Exception("Ошибка в try-catch (вариант 3)");
            }
            catch (Exception exception)
            {
                LogManager.GetCurrentClassLogger().Error(exception);
            }
            return View();         
        }
    }
}