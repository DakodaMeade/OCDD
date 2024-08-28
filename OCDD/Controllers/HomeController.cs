using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using System.Diagnostics;
/*
 * Dakoda Meade	
 * Home Controller
 * Manages the home page
 */
namespace OCDD.Controllers
{
	public class HomeController : Controller
	{
		//private readonly ILogger<HomeController> _logger;

		//public HomeController(ILogger<HomeController> logger)
		//{
		//	_logger = logger;
		//}


		/// <summary>
		/// Shows the home page
		/// </summary>
		/// <returns>Returns the home index view</returns>
		public IActionResult Index()
		{
			return View();
		}

		
		/// <summary>
		/// Shows the error page
		/// </summary>
		/// <returns></returns>
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
