using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NLog;
using OCDD.Services;
using OCDD.Services.Utility;
using OCDD.Controllers;
using OCDD.Models;
using Org.BouncyCastle.Bcpg;

/*
 * Dakoda Meade
 * CST-350
 * Activity 6-1
 * Controller for the login view 
 * this proccess the entered information and displays a new view based on if the user is valid or not
 */
namespace OCDD.Controllers
{
	public class LoginController : Controller
	{
		// logger object
		//private static Logger logger = LogManager.GetLogger("LoginAppLoggerrule");



		// Login/Index
		public IActionResult Index()
		{
            //logger.Info("Entering the process login method");
            //MyLogger.GetInstance().Info("Entering the process login method");
            return View();
		}
		



		[HttpPost]
		[LogActionFilter]
		public IActionResult ProcessLogin(UserModel user)
		{
			
			SecurityService securityService = new SecurityService();
			if (securityService.IsValid(user))
			{
				

				// remember who logged in
				int userID = securityService.GetUserId(user);
				HttpContext.Session.SetString("userID", userID.ToString());
                user = securityService.GetUserById(userID);
                user.userID = userID;
                HttpContext.Session.SetString("userRole", user.role);
                return View("LoginSuccess", user);
			}
			else
			{
				

				HttpContext.Session.Remove("userID");
				return View("LoginFailure", user);
			}
		}
	}
}
