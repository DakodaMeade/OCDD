using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;
/*
 * Dakoda Meade
 * Appointment Contoller
 * This controller is for managing appointments.
 */
namespace OCDD.Controllers
{
    
    public class AppointmentController : Controller
    {
        AppointmentService appointmentService = new AppointmentService(); // initialize appointment service
       
        /// <summary>
        /// Retrives appointment and sends the appointment object to the index view.
        /// </summary>
        /// <param name="appointmentID"> Id of the appointment selcted</param>
        /// <param name="source"> The page where link to index page was clicked</param>
        /// <returns>Appointment Index view</returns>
        [CustomAuthorization]
        public IActionResult Index(int appointmentID, string source)
        {
            //gets all appointment inforamtion based on the id
            AppointmentModel appointment = appointmentService.GetAppointment(appointmentID);
            // Pass the source parameter to the view to detirmine what to display
            ViewData["Source"] = source;
            return View(appointment);
        }


        /// <summary>
        /// Sets the appointment to complete
        /// </summary>
        /// <param name="appointmentID"> Id of the appointment </param>
        /// <returns> Returns back to the admin dashboard.</returns>
        /// 
        [AdminAuthorization]
        [HttpPost]
        public IActionResult CompleteAppointment(int appointmentID)
        {
            // sets appoitnemtn to complete
            appointmentService.CompleteAppointment(appointmentID);
            return RedirectToAction("Index", "AdminDashboard");
        }
        /// <summary>
        /// Cancels appointment
        /// </summary>
        /// <param name="appointmentID"> Id of the appointment </param>
        /// <param name="source"> The page where link to the page was clicked </param>
        /// <returns> Returns to the source </returns>
        [CustomAuthorization]
        [HttpPost]
        public IActionResult CancelAppointment(int appointmentID, string source)
        {
            // cancels appointment
            appointmentService.CancelAppointment(appointmentID);
            if (source == "MyAppointments")
            {
                return RedirectToAction("Index", "MyAppointments"); // returns to the my appointments page
            }
            else 
            {
                return RedirectToAction("Index", "AdminDashboard"); // returns to the adin dashboard page
            }
        }

    }
}
