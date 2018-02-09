using System;
using NLog;
using Zidium.Api;

namespace Zidium.Examples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Получим логгер
            var logger = LogManager.GetCurrentClassLogger();

            // Отправим запись в лог о старте приложения
            logger.Info("Приложение запущено");

            // Запишем в лог предупреждение с параметрами
            var errorCode = 1000;
            var data = new byte[100];
            var logEvent = new LogEventInfo()
            {
                Level = NLog.LogLevel.Warn,
                Message = "Не удалось получить данные"
            };
            logEvent.Properties.Add("Код ошибки", errorCode);
            logEvent.Properties.Add("Содержимое пакета", data);
            logger.Log(logEvent);

            // Поймаем исключение и залогируем ошибку
            try
            {
                throw new Exception("Test exception");
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }

            // Получим компонент Zidium
            // Настройки авторизации укажите в разделе <access> файла Zidium.config
            // ID компонента укажите в разделе <defaultComponent>
            var component = Client.Instance.GetDefaultComponentControl();

            // Обновим компоненту версию
            component.Update(new UpdateComponentData() { Version = Version });

            // Получим проверку
            var unitTest = component.GetOrCreateUnitTestControl("Zidium.Example.ActivityUnitTest");

            // Отправим проверку, актуальную 10 минут
            unitTest.SendResult(UnitTestResult.Success, TimeSpan.FromMinutes(10), "Приложение работает нормально");

            // Отправим метрику - свободное место на диске, актуальную 10 минут
            component.SendMetric("HDD", 1024, TimeSpan.FromMinutes(10));

            // Перед завершением приложения обязательно вызовем отправку закешированных данных
            Client.Instance.Flush();
        }

        private static string Version
        {
            get { return typeof(Program).Assembly.GetName().Version.ToString(); }
        }
    }
}
