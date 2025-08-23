using Microsoft.AspNetCore.Mvc;

namespace Training_Management_System_ITI_Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
