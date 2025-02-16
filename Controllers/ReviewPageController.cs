using Microsoft.AspNetCore.Mvc;

namespace PassionProject.Controllers
{
    public class ReviewPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
