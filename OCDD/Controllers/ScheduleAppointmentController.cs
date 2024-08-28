using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;
/*
 * Dakoda Meade
 * Schedule Appoitnemtn Controller
 * Manages the scheduling appointments and their views
 * 
 */

namespace OCDD.Controllers
{
    public class ScheduleAppointmentController : Controller
    {
        AppointmentService appointmentService = new AppointmentService();
        ServiceService serviceService = new ServiceService();


        /// <summary>
        /// Display the scheduling form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            //AppointmentService appointmentService = new AppointmentService();
            // Get the list of services from the database
            ViewBag.Services = serviceService.GetServices();
            return View();
        }

        /// <summary>
        /// Handle the form submission
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Schedule(AppointmentModel appointment)
        {
            
             // Combine date and time
             DateTime date = appointment.date;
             TimeSpan time = appointment.time;
             appointment.dateTime = date.Add(time);

            // initailize appointment ID with 0
            int appointmentID = 0 ;

            // if user is register
            if (HttpContext.Session.GetString("userID") != null)
            {
                int userID = Int32.Parse(HttpContext.Session.GetString("userID"));
                appointment.user = new UserModel { userID = userID };
                // saves appoitnemnt for user to DB
                appointmentID = appointmentService.SaveAppointmentUser(appointment);
            }
            else
            {
                // saves appoitnemtn for nonuser To DB
                appointmentID = appointmentService.SaveAppointmentNonUser(appointment);
                
            }
            // get the appoitnemtn information to display on confirmation page
            appointment = appointmentService.GetAppointment(appointmentID);
            // displays appoitnemtn confirmatino page
            return View("AppointmentConfirmation", appointment);
        }
        /// <summary>
        /// Returns the available time slots with client side 
        /// </summary>
        /// <param name="date">users date selected</param>
        /// <param name="serviceDuration"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAvailableTimeSlots(DateTime date, string serviceDuration)
        {
            try
            {
                // Convert the serviceDuration string to TimeSpan
                if (!TimeSpan.TryParse(serviceDuration, out TimeSpan parsedServiceDuration))
                {
                    return BadRequest("Invalid service duration format.");
                }
                // stores available timeslots in var
                var availableSlots = appointmentService.GetAvailableTimeSlots(date, parsedServiceDuration);
                // convert time slots to 12 hour clock
                var timeSlots = availableSlots.Select(slot => slot.ToString("HH:mm")).ToList();

                return Json(timeSlots); // Return time slots as JSON
            }
            catch (Exception ex)
            {
                // Handle the error (e.g., log it and return an error response)
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while retrieving time slots.");
            }
        }

        

        
    }
}

