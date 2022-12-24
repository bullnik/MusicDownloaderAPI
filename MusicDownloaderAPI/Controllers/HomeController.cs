using Microsoft.AspNetCore.Mvc;
using MusicDownloaderAPI.Models;
using MusicDownloaderAPI.Rabbit;
using System.Diagnostics;

namespace MusicDownloaderAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRabbitMqService _rabbitmq;

        public HomeController(ILogger<HomeController> logger, IRabbitMqService rabbitMqService)
        {
            _logger = logger;
            _rabbitmq = rabbitMqService;
        }

        [HttpGet]
        public async Task<IActionResult> Download(string link)
        {
            return NotFound();
        }

        public IActionResult Index()
        {
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