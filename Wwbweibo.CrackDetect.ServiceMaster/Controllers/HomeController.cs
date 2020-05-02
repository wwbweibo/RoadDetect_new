using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Wwbweibo.CrackDetect.ServiceMaster.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster.Controllers
{
    public class HomeController : Controller
    {
        private readonly MasterService service;
        private readonly TaskService taskService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MasterService masterService, TaskService taskService)
        {
            _logger = logger;
            this.service = masterService;
            this.taskService = taskService;
        }

        public IActionResult Index()
        {
            ViewData["tasks"] = taskService.GetAllTasks();
            return View();
        }

        public IActionResult Detail(Guid id)
        {
            ViewData["item"] = taskService.GetTaskDetails(id);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
