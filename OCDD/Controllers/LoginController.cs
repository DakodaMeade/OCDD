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
 *	Login Controller
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
		// display the login form page
		public IActionResult Index()
		{
            //logger.Info("Entering the process login method");
            //MyLogger.GetInstance().Info("Entering the process login method");
            return View();
		}
		


		/// <summary>
		/// Processes the page to display when a user logs in
		/// </summary>
		/// <param name="user">user object</param>
		/// <returns>The resulting view</returns>
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
				// get the user for the databsae
                user = securityService.GetUserById(userID);
                user.userID = userID;
                HttpContext.Session.SetString("userRole", user.role);// set the users role
                return View("LoginSuccess", user);
			}
			else
			{
				
				// log user out and display failure page
				HttpContext.Session.Remove("userID");
				return View("LoginFailure", user);
			}
		}
	}
}
