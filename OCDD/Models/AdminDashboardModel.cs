namespace OCDD.Models
{
    /// <summary>
    /// Dakoda Meade
    /// This class is the view model for the admin dashboard
    /// Contains lists of the service model and the appoitnemtns model
    /// </summary>
    public class AdminDashboardModel
    {
        public List<ServiceModel> services { get; set; }
        public List<AppointmentModel> appointments { get; set; }


    }
}
