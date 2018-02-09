using System;
using System.Threading;
using System.Web.Mvc;
using NLog;
using Zidium.Api;
using Zidium.Examples.Models;
using LogLevel = NLog.LogLevel;

namespace Zidium.Examples.Controllers
{
    /// <summary>
    /// Контроллер для примеров записи логов
    /// </summary>
    public class LogController : Controller
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Простой пример записи лога
        /// </summary>
        public ActionResult SimpleExample()
        {
            _logger.Trace("Пример лога Trace");
            _logger.Debug("Пример лога Debug");
            _logger.Info("Пример лога Info");
            _logger.Warn("Пример лога Warning");
            _logger.Error("Пример лога Error");
            _logger.Fatal("Пример лога Fatal");

            var model = new LogModel()
            {
                ZidiumComponentIsFake = Client.Instance.GetDefaultComponentControl().IsFake()
            };

            if (!model.ZidiumComponentIsFake)
            {
                model.ComponentId = Client.Instance.GetDefaultComponentControl().Info.Id;
                model.AccountName = Client.Instance.Config.Access.AccountName;
            }

            return View(model);
        }

        /// <summary>
        /// Пример записи сообщения лога с дополнительными свойствами
        /// </summary>
        public ActionResult PropertiesExample()
        {
            var logEvent = new LogEventInfo()
            {
                Level = LogLevel.Info,
                Message = "Эта запись содержит дополнительные свойства"
            };
            logEvent.Properties.Add("User", HttpContext.User.Identity.Name);
            logEvent.Properties.Add("ApplicationPath", Request.PhysicalApplicationPath);
            _logger.Log(logEvent);

            var model = new LogModel()
            {
                ZidiumComponentIsFake = Client.Instance.GetDefaultComponentControl().IsFake()
            };

            if (!model.ZidiumComponentIsFake)
            {
                model.ComponentId = Client.Instance.GetDefaultComponentControl().Info.Id;
                model.AccountName = Client.Instance.Config.Access.AccountName;
            }

            return View(model);
        }

        /// <summary>
        /// Пример использования контекста лога
        /// </summary>
        public ActionResult ContextExample()
        {
            int count = 10; // количество транзакций
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                var transactionId = "transactionId=" + i;
                var thread = new Thread(() =>
                {
                    var contextLogger = LogManager.GetLogger(transactionId);
                    contextLogger.Info("Начинаем обработку транзакции " + transactionId);
                    Thread.Sleep(random.Next(20));
                    contextLogger.Info("Входные данные уcпешно проверены");
                    Thread.Sleep(random.Next(20));
                    contextLogger.Info("Денежные средства успешно заблокированы");
                    Thread.Sleep(random.Next(20));
                    if (random.Next() % 2 == 0)
                    {
                        contextLogger.Info("Получено подтверждение проведения транзакции");
                    }
                    else
                    {
                        contextLogger.Warn("Отказано в проведении транзакции");
                    }
                    contextLogger.Info("Обработка транзакции завершена");

                });
                thread.Start();
            }

            var model = new LogModel()
            {
                ZidiumComponentIsFake = Client.Instance.GetDefaultComponentControl().IsFake()
            };

            if (!model.ZidiumComponentIsFake)
            {
                model.ComponentId = Client.Instance.GetDefaultComponentControl().Info.Id;
                model.AccountName = Client.Instance.Config.Access.AccountName;
            }

            return View(model);
        }
    }
}