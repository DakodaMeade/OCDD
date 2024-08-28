using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using OCDD.Models;
using OCDD.Services;
/*
 * Dakoda Meade
 * Service Controller
 * Manages services including edit updating adding and deleting
 */
namespace OCDD.Controllers
{
    [AdminAuthorization]
    public class ServiceController : Controller
    {
        // set up services
        ServiceService serviceService = new ServiceService();
        /// <summary>
        /// 
        /// </summary>
        /// <returns> Returns service Index View</returns>
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
