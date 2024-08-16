using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCDD.Models;
using OCDD.Services;
/*
 
 Dakoda Meade
 Controller for managing filtering for appointments
 Utilizes the shared appointments partial view
 
 */

namespace OCDD.Controllers
{
    public class AppointmentsFilterController : Controller
    {
        SecurityService securityService = new SecurityService();
        AppointmentService appointmentService = new AppointmentService();
        //List<AppointmentModel> appointments = new List<AppointmentModel>();
        

        [HttpPost]
        public IActionResult Filter(string status, int appointmentId, DateTime? startDate, DateTime? endDate, string source)
        {
            IEnumerable<AppointmentModel> appointments;

            if (source == "MyAppointments")
            {
                // gets the user that is logged in 
                int userId = Convert.ToInt32(HttpContext.Session.GetString("userID"));
                UserModel user = securityService.GetUserById(userId);
                user.userID = userId;

                appointments = appointmentService.GetAppointmentsByUserID(userId);

                appointments = appointments
                    .Where(a => a.status == "Completed" || a.status == "Cancelled") // past appointments
                    .ToList();

            }
            else
            {
                appointments = appointmentService.GetAllAppointments();
            }
            //var Appointments = // Fetch your data context

            // Apply filters based on the provided parameters
            if (startDate.HasValue)
            {
                appointments = appointments.Where(a => a.dateTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                // Set endDate time to 11:59 PM
                var endDateTime = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                appointments = appointments.Where(a => a.dateTime <= endDateTime);
            }

            if (!string.IsNullOrEmpty(status) && status != "All")
            {

                appointments = appointments.Where(a => a.status == status); 
            }

            if (appointmentId > 0)
            {
                appointments = appointments.Where(a => a.appointmentID == appointmentId); // Adjust based on how you search by ID
            }
            appointments.OrderByDescending(a => a.dateTime);
            // Get filtered results
            var filteredAppointments = appointments.ToList();
            return PartialView("_AppointmentsPartial", filteredAppointments);
        }


    }
}
