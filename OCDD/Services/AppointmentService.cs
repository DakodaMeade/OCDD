using OCDD.Models;
/*
 * Dakoda Meade
 * Appointment Service Class
 * Handles interaction between controllers and DAO classes
 */
namespace OCDD.Services
{
    public class AppointmentService
    {
        // DAO class initialize
        AppointmentDAO appointmentDAO = new AppointmentDAO();



        // Save appoitnement for logged in user
        public int SaveAppointmentUser(AppointmentModel appointment)
        {
            return appointmentDAO.SaveAppointmentUser(appointment);
        }
        // save appoitnemtn for non logged in user
        public int SaveAppointmentNonUser(AppointmentModel appointment)
        {
            return appointmentDAO.SaveAppointmentNonUser(appointment);
        }
        // return the appoitnment based on the appointment id
        public AppointmentModel GetAppointment(int appointmentID)
        {
            return appointmentDAO.GetAppointmentByID(appointmentID);
        }
        // return the appoitnment based on the appointment id
        public List<AppointmentModel> GetAllAppointments()
        {
            return appointmentDAO.GetAllAppointments();
        }
        // retruns all the appointments based on the user id
        public List<AppointmentModel> GetAppointmentsByUserID(int userID)
        {
            return appointmentDAO.GetAppointmentsByUserID(userID);
        }

         //update all appoinemtsn with matching emial of the user
        public void UpdateAppointmentsWithUserID(string email, int userID) 
        {
            appointmentDAO.UpdateAppointmentsWithUserID(email, userID);
        }

        //Cancel appointment 
        public void CancelAppointment(int appointmentID)
        {
            appointmentDAO.CancelAppointment(appointmentID);
        }

        //Complete appointment 
        public void CompleteAppointment(int appointmentID)
        {
            appointmentDAO.CompleteAppointment(appointmentID);
        }

        //public List<DateTime> GetAvailableTimeSlots(DateTime dateTime)
        //{
        //   return appointmentDAO.GetAvailableTimeSlots(dateTime);
        //}
        // return all times slots aviable based on the proviced information
        public List<DateTime> GetAvailableTimeSlots(DateTime dateTime, TimeSpan serviceDuration)
        {
            return appointmentDAO.GetAvailableTimeSlots(dateTime, serviceDuration);
        }
    }
}
