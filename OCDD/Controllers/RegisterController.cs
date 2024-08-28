using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;
/*
 * Dakoda Meade
 * Register Controller
 * This handles the user registration
 * 
 */
namespace OCDD.Controllers
{
    public class RegisterController : Controller
    {

        /// <summary>
        /// Display the registration form
        /// </summary>
        /// <returns>Register form page</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets information of the non registered user fromt the appointment confirmation page and prefills the registration page.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="address"></param>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <returns>The registration page with the prefilled user info</returns>
        public IActionResult RegisterFromAppointment(string name, string email, string phoneNumber, string address, int zipCode, string city, string state)
        {
            // Create an instance of UserModel
            var user = new UserModel
            {
                name = name,
                email = email,
                phoneNumber = phoneNumber,
                address = address,
                zipCode = zipCode,
                city = city,
                state = state
            };
            

            return View("Index", user); // Returns registration form views with  user info
        }


        /// <summary>
        ///  This handles the post register view
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The failure or success register views</returns>

        public IActionResult ProcessRegister(UserModel user)
        {
            SecurityService securityService = new SecurityService();
            // Checks if the emails exists in the database
            if (!securityService.UserExists(user))
            {
                //SQL for adding a new user since register success
                securityService.AddUser(user);
                // register/ProcessRegister
                return View("RegisterSuccess", user);
            }
            else
            {
                // register/ProcessRegister
                return View("RegisterFailure", user);
            }
        }
    }
}
