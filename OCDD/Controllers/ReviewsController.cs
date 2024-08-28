using Microsoft.AspNetCore.Mvc;
/*
 * Dakoda Meade
 * Reviews Controller
 * Handles the Reviews view page
 * All reviews are hard coded 
 */
namespace OCDD.Controllers
{
    public class ReviewsController : Controller
    {
        /// <summary>
        /// Display the reviews index view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
