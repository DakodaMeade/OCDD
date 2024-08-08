using Microsoft.AspNetCore.Mvc;
using OCDD.Models;
using OCDD.Services;

namespace OCDD.Controllers
{
    public class AdminDashboardController : Controller
    {
        ServiceService serviceService = new ServiceService();
        AppointmentService appointmentService = new AppointmentService();
        AdminDashboardModel adminDash = new AdminDashboardModel();
        public IActionResult Index()
        {
            adminDash.services = serviceService.GetServices();
            adminDash.appointments = appointmentService.GetAllAppointments();
            return View(adminDash);
        }

       
    }
}
