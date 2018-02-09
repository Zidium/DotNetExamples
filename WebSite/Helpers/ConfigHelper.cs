using System;
using System.Configuration;

namespace Zidium.Examples.Helpers
{
    public static class ConfigHelper
    {
        /// <summary>
        /// Проверяет, что параметр есть в AppSettings
        /// </summary>
        /// <param name="name"></param>
        public static void ValidateAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("В AppSettings не задан обязательный параметр " + name);
            }
        }
    }
}