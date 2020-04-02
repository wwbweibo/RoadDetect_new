using Microsoft.AspNetCore.Mvc;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class LogController : Controller
    {
        private LogService logService;

        public LogController(LogService logService)
        {
            this.logService = logService;
        }
        public IActionResult Index()
        {
            ViewData["logs"] = logService.GetAllLogMessages();
            return View();
        }
    }
}