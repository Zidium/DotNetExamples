using System;
using System.Web.Mvc;
using Zidium.Api;
using Zidium.Examples.Models;

namespace Zidium.Examples.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            var model = new HomeModel()
            {
                ZidiumComponentIsFake = Client.Instance.GetDefaultComponentControl().IsFake()
            };

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ConnectionTest()
        {
            var model = new ConnectionTestModel()
            {
                ZidiumComponentIsFake = Client.Instance.GetDefaultComponentControl().IsFake(),
            };

            if (!model.ZidiumComponentIsFake)
            {
                model.ComponentId = Client.Instance.GetDefaultComponentControl().Info.Id;

                var response = Client.Instance.ApiService.GetEcho(Guid.NewGuid().ToString());
                model.IsResponseSuccess = response.Success;
                model.ResponseCode = response.Code;
                model.ResponseErrorMessage = response.ErrorMessage;
            }

            return View(model);
        }        
    }
}