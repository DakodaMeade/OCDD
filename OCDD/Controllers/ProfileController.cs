using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OCDD.Models;
using OCDD.Services;

namespace OCDD.Controllers
{
    public class ProfileController : Controller
    {
        SecurityService securityService = new SecurityService();

        [CustomAuthorization]
        public IActionResult Index()
        {
            //SecurityService securityService = new SecurityService();
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            UserModel user = securityService.GetUserById(userId);
            user.userID = userId;
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }
        [CustomAuthorization]
        public IActionResult UpdateProfile(UserModel user)
        {
            
            securityService.UpdateProfile(user);
            return View("Index", user);
        }
        [CustomAuthorization]
        public IActionResult UpdatePassword(UserModel user)
        {
           
            securityService.UpdatePassword(user); 
            //int userId = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            user = securityService.GetUserById(user.userID);
            user.password = "";
            //user.userID = userId;
            return View("Index", user);
        }

        /// <summary>
        /// Method to log user out
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {


             HttpContext.Session.Remove("userID");
             return RedirectToAction("Index", "Home");

        }
    }
}
