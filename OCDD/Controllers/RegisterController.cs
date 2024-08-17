using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;
/*
 * CST-350
 * Register Controller
 * This handles the user registration
 * 
 */
namespace OCDD.Controllers
{
    public class RegisterController : Controller
    {

        // register view
        public IActionResult Index()
        {
            return View();
        }

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
            

            // Process the appointment model here

            return View("Index", user); // Or redirect to another action/view
        }


        //   register/ProcessRegister
        // This handles the post register view
        public IActionResult ProcessRegister(UserModel user)
        {
            SecurityService securityService = new SecurityService();
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
