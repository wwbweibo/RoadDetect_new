using Microsoft.AspNetCore.Mvc;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class LogController : Controller
    {
        private LogService logService = new LogService(Program.GetRedisClient());
        public IActionResult Index()
        {
            ViewData["logs"] = logService.GetAllLogMessages();
            return View();
        }
    }
}