/*
 * Dakoda Meade
 * Service service class working with service information in database
 */
using OCDD.Models;

namespace OCDD.Services
{

    public class ServiceService
    {
        // set up service DAO 
        ServiceDAO serviceDAO = new ServiceDAO();
        // returns the service with matching ID 
        public ServiceModel GetServiceByID(int serviceID)
        {
            return serviceDAO.GetServiceByID(serviceID);
        }

        // returns all services
        public List<ServiceModel> GetServices()
        {
            return serviceDAO.GetServices();
        }

        // Saves service
        public void SaveService(ServiceModel service) 
        {
            serviceDAO.SaveService(service);
        }

        // Soft deletes service
        public void DeleteService(int serviceID)
        {
            serviceDAO.DeleteService(serviceID);
        }
    }
}
