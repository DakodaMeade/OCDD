using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;


namespace OCDD.Controllers
{
    public class ScheduleAppointmentController : Controller
    {
        AppointmentService appointmentService = new AppointmentService();
        ServiceService serviceService = new ServiceService();
        // Display the scheduling form
        [HttpGet]
        public IActionResult Index()
        {
            //AppointmentService appointmentService = new AppointmentService();
            // Get the list of services from the database
            ViewBag.Services = serviceService.GetServices();
            return View();
        }

        // Handle the form submission
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
                appointmentID = appointmentService.SaveAppointmentUser(appointment);

                
            }
            else
            {
                appointmentID = appointmentService.SaveAppointmentNonUser(appointment);
                
            }
            appointment = appointmentService.GetAppointment(appointmentID);
            return View("AppointmentConfirmation", appointment);
        }

        [HttpGet]
        public IActionResult GetAvailableTimeSlots(DateTime date)
        {
            try
            {
                var availableSlots = appointmentService.GetAvailableTimeSlots(date);
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



        // Display the confirmation page
        public IActionResult Confirmation()
        {
            return View();
        }

        

        
    }
}

