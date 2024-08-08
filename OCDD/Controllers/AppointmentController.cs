using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;

namespace OCDD.Controllers
{
    public class AppointmentController : Controller
    {
        AppointmentService appointmentService = new AppointmentService();
        public IActionResult Index(int appointmentID, string source)
        {
            AppointmentModel appointment = appointmentService.GetAppointment(appointmentID);
            // Pass the source parameter to the view to detirmine what to display
            ViewData["Source"] = source;
            return View(appointment);
        }
        /// <summary>
        /// Method to set the appointment to complete
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CompleteAppointment(int appointmentID)
        {
            appointmentService.CompleteAppointment(appointmentID);
            return RedirectToAction("Index", "MyAppointments");
        }
        /// <summary>
        /// Method to cancel appointment
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CancelAppointment(int appointmentID)
        {
            appointmentService.CancelAppointment(appointmentID);
            return RedirectToAction("Index", "MyAppointments");
        }

    }
}
