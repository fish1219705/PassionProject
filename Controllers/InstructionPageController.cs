using Microsoft.AspNetCore.Mvc;

namespace PassionProject.Controllers
{
    public class InstructionPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
