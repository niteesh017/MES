using Microsoft.AspNetCore.Mvc;

namespace MESDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
