using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class ServiceController : Controller
    {
        private MasterService service = new MasterService();
        public IActionResult Index()
        {
            var AllService = service.ListAllRegisteredService();
            ViewData["services"] = AllService;
            return View();
        }

        public JsonResult StopService(string serviceId, ServiceType serviceType)
        {
            return Json(new Dictionary<string,string>(){{"Result",service.StopService(serviceType, serviceId).ToString()}});
        }
    }
}