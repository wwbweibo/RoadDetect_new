using Microsoft.AspNetCore.Mvc;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService taskService;

        public TaskController(TaskService service)
        {
            this.taskService = service;
        }

        public IActionResult Index()
        {
            ViewData["tasks"] = taskService.ListAllTodoTask();
            return View();
        }
    }
}