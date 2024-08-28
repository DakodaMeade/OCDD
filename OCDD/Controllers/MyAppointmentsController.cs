using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;
using System.Collections.Generic;
/*
 * Dakoda Meade
 * My Appointments Controller
 * It displays all the appointments of the user. Upcomming and past appoitnments. Allowing the user to view the details of the appoitnemtn
 * 
 */
namespace OCDD.Controllers
{
    [CustomAuthorization]
    public class MyAppointmentsController : Controller
    {
        // set up services
        SecurityService securityService = new SecurityService();
        AppointmentService appointmentService = new AppointmentService();

        /// <summary>
        /// Displays the My appointment page with appointment info
        /// </summary>
        /// <returns>Appointment index view</returns>
        public IActionResult Index()
        {
            // gets the user that is logged in 
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            UserModel user = securityService.GetUserById(userId);
            user.userID = userId;

            // updates the database if the user has made any appointments not logged in by email
            appointmentService.UpdateAppointmentsWithUserID(user.email, user.userID);

            //return all the users appointments into a temporary list
            List <AppointmentModel> allAppointments = appointmentService.GetAppointmentsByUserID(userId);


            // Separate into upcoming and past appointments using LINQ
            var upcomingAppointments = allAppointments
                .Where(a => a.status == "Scheduled") // Assuming "Scheduled" is the status for upcoming appointments
                .OrderBy(a => a.dateTime)
                .ToList();

            var pastAppointments = allAppointments
                .Where(a => a.status == "Completed" || a.status == "Cancelled") // past appointments
                .OrderByDescending(a => a.dateTime) // Sort by newest to oldest
                .ToList();
            // set to the objects of the user
            user.upcomingAppointments = upcomingAppointments;
            user.pastAppointments = pastAppointments;

            // return the view with the user.
            return View(user);
        }
    }
}
