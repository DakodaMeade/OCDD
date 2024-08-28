using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;
/* Dakoda Meade
 * Controller for the Admin dashboard.
 * 
 */ 
namespace OCDD.Controllers
{
    // requires admin authorization to access
    [AdminAuthorization]
    public class AdminDashboardController : Controller
    {
        // Services objects and admn dash model
        ServiceService serviceService = new ServiceService();
        AppointmentService appointmentService = new AppointmentService();
        // to display all info for the admin dashboard
        AdminDashboardModel adminDash = new AdminDashboardModel();

        /// <summary>
        /// REtrieves the services and appointments to pass to the view.
        /// </summary>
        /// <returns> The admin dash index view, passing the admin dash object. </returns>
        public IActionResult Index()
        {
            adminDash.services = serviceService.GetServices();
            adminDash.appointments = appointmentService.GetAllAppointments();
            return View(adminDash);
        }

       
    }
}
