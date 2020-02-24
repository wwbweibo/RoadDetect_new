using Microsoft.AspNetCore.Mvc;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class TaskController : Controller
    {
        private MasterService service = new MasterService();
        public IActionResult Index()
        {
            ViewData["tasks"] = service.ListAllTodoTask();
            return View();
        }
    }
}