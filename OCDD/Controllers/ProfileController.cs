using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OCDD.Models;
using OCDD.Services;
/*
 * Dakoda Meade
 * Profile controller
 * Manages user profile. Allowing user to edit their information or even log out at any time
 */
namespace OCDD.Controllers
{
    public class ProfileController : Controller
    {
        // set up services
        SecurityService securityService = new SecurityService();

        /// <summary>
        /// Get user to display the profile page.
        /// </summary>
        /// <returns>User profile page view</returns>
        [CustomAuthorization]
        public IActionResult Index()
        {
            //SecurityService securityService = new SecurityService();
            // save user id
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            //get the user object of the logged in user with the user id
            UserModel user = securityService.GetUserById(userId);
            user.userID = userId;
            // returns the user profile paged if logged in 
            //this may be reduntant after implmenting the custom authorization
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Updates the user profile information
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns>Reloads the index view with new user info</returns>
        [CustomAuthorization]
        public IActionResult UpdateProfile(UserModel user)
        {
            // pdate the user in the databasae with the new inforation fromt he user
            securityService.UpdateProfile(user);
            return View("Index", user);
        }

        /// <summary>
        /// Updates only the users password
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        [CustomAuthorization]
        public IActionResult UpdatePassword(UserModel user)
        {
           // updates the password in the database
            securityService.UpdatePassword(user); 
            //int userId = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            // gets the user infomation to reload
            user = securityService.GetUserById(user.userID);
            // makes the password blank so we do not get an error trying to display
            user.password = "";
            //user.userID = userId;
            return View("Index", user);
        }

        /// <summary>
        /// Logs the user out
        /// </summary>
        /// <returns>to the home screen</returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            // logs out the user
             HttpContext.Session.Remove("userID");
             return RedirectToAction("Index", "Home");

        }
    }
}
