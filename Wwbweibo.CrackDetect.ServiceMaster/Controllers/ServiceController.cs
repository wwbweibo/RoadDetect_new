using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using StackExchange.Redis;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class ServiceController : Controller
    {
        private MasterService _masterService;

        public ServiceController(MasterService service)
        {
            this._masterService = service;
        }

        public IActionResult Index()
        {
            var AllService = _masterService.ListAllRegisteredService();
            ViewData["services"] = AllService;
            return View();
        }

        public JsonResult StopService(string serviceId, ServiceType serviceType)
        {
            return Json(new Dictionary<string, string>() { { "Result", _masterService.StopService(serviceType, serviceId).ToString() } });
        }

        public IActionResult StartDataCollect(string serviceId)
        {
            _masterService.StartDataCollect(serviceId);
            return Redirect("/Service");
        }

        public IActionResult StopDataCollect(string serviceId)
        {
            _masterService.StopDataCollect(serviceId);
            return Redirect("/Service");
        }
    }
}