using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using OCDD.Models;
using OCDD.Services;

namespace OCDD.Controllers
{
    [AdminAuthorization]
    public class ServiceController : Controller
    {
        ServiceService serviceService = new ServiceService();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditService(int serviceID)
        {
            ServiceModel service = serviceService.GetServiceByID(serviceID);

            
            return View("Index", service);
        }

        public IActionResult SaveService(ServiceModel service)
        {
            
            serviceService.SaveService(service);

            return RedirectToAction("Index", "AdminDashboard");
        }

        public IActionResult DeleteService(int serviceID)
        {

            serviceService.DeleteService(serviceID);

            return RedirectToAction("Index", "AdminDashboard");
        }
    }
}
