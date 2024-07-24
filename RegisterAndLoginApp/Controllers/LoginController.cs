using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RegisterAndLoginApp.Models;
using RegisterAndLoginApp.Services;
using NLog;
using RegisterAndLoginApp.Services.Utility;
/*
 * Dakoda Meade
 * CST-350
 * Activity 6-1
 * Controller for the login view 
 * this proccess the entered information and displays a new view based on if the user is valid or not
 */
namespace RegisterAndLoginApp.Controllers
{
    public class LoginController : Controller
    {
        // logger object
        //private static Logger logger = LogManager.GetLogger("LoginAppLoggerrule");



        // Login/Index
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [CustomAuthorization]
        public IActionResult PrivateSectionMustBeLoggedIn()
        {
            return Content("I am a protected method if the proper attribute is applied to me.");
        }



        [HttpPost]
        [LogActionFilter]
        public IActionResult ProcessLogin(UserModel user) 
        {
            //logger.Info("Entering the process login method");
            //logger.Info("Parameter: " + user.ToString());
            //MyLogger.GetInstance().Info("Entering the process login method");
            //MyLogger.GetInstance().Info("Parameter: " + user.ToString());
            SecurityService securityService = new SecurityService();
            if (securityService.IsValid(user))
            {
                // login/loginsuccess
                //logger.Info("Login Success");
                MyLogger.GetInstance().Info("Login Success");

                // remember who logged in
                //HttpContext.Session.SetString("username", user.username);
                return View("LoginSuccess", user);
            }
            else
            {
                // login/loginfailure
                //logger.Info("Login Failure");
                //MyLogger.GetInstance().Info("Login Failure");

                HttpContext.Session.Remove("username");
                return View("LoginFailure", user);
            }
        }
    }
}
