using System.Web;
using Zidium.Api;

namespace Zidium.Examples.Helpers
{
    /// <summary>
    /// Заполняет свойства событий (ошибок) параметрами HTTP-запроса
    /// </summary>
    public class EventPreparer : IEventPreparer
    {
        public void Prepare(SendEventBase eventObj)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return;
            }
            if (httpContext.Handler == null)
            {
                // чтобы не было исключения при старте приложения,
                // когда httpContext.Request пустой.
                return;
            }
            var request = httpContext.Request;
            eventObj.Properties.Set("IP", request.UserHostAddress);
            eventObj.Properties.Set("UserAgent", request.UserAgent);
            eventObj.Properties.Set("Url", request.Url.AbsoluteUri);
            eventObj.Properties.Set("UrlReferrer", request.UrlReferrer);
            eventObj.Properties.Set("Login", httpContext.User.Identity.Name);
        }
    }
}