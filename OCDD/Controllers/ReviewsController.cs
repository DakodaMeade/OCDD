using Microsoft.AspNetCore.Mvc;

namespace OCDD.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
