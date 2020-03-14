using Microsoft.AspNetCore.Mvc;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class TaskController : Controller
    {
        private MasterService service;

        public TaskController(MasterService service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            ViewData["tasks"] = service.ListAllTodoTask();
            return View();
        }
    }
}