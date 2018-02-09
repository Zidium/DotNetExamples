using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Zidium.Api;
using Zidium.Examples.Helpers;

namespace Zidium.Examples.Controllers
{
    /// <summary>
    /// Контроллер содержит страницу selftest - страница самопроверки
    /// </summary>
    public class SelfTestController : Controller
    {     
        /// <summary>
        /// Проверяет, что web.config содержит все обязательные настройки
        /// </summary>
        private void ConfigTest()
        {
            ConfigHelper.ValidateAppSettings("SmtpHost");
            ConfigHelper.ValidateAppSettings("SmtpPort");
        }

        /// <summary>
        /// Проверяет, что есть соединение с БД
        /// </summary>
        private void SqlTest()
        {
            // реализуйте проверку самостоятельно
        }

        /// <summary>
        /// Проверяет, что есть соединение с системой мониторинга Zidium
        /// </summary>
        private void ZidiumTest()
        {
            var client = Client.Instance;
            client.ApiService.GetEcho(Guid.NewGuid().ToString()).Check();
        }

        /// <summary>
        /// Проверяет, что есть соединение с WCF-сервисом
        /// </summary>
        private void WcfServiceTest()
        {
            // реализуйте проверку самостоятельно
        }

        /// <summary>
        /// Страница самопроверки проверяет:
        /// - web.config содержит все обязательные настройки
        /// - есть соединение с базой данных
        /// - есть соединение с Zidium
        /// - есть соединение с WCF сервисом
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Здесь можно реализовать какие угодно проверки, чем проверок больше, тем лучше.
            var tests = new Dictionary<string, Action>();
            tests.Add("ConfigTest", ConfigTest);
            tests.Add("SqlTest", SqlTest);
            tests.Add("ZidiumTest", ZidiumTest);
            tests.Add("WcfServiceTest", WcfServiceTest);

            bool success = true;
            var log = new StringBuilder();
            foreach(var testPair in tests)
            {
                var testAction = testPair.Value;
                var testName = testPair.Key;
                log.AppendLine("----------------------------------");
                try
                {
                    
                    testAction();
                    log.AppendLine(testName + ": success");
                }
                catch (Exception exception)
                {
                    success = false;
                    log.AppendLine(testName + ": error : " + exception.Message);
                }
            }
            var response = success ? "###### SUCCESS ######" : "###### ERROR ######";
            response = response + Environment.NewLine + log;
            return Content(response, "text/plain");
        }
    }
}