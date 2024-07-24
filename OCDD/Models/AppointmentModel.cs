namespace OCDD.Models
{
	/*
* Dakoda Meade
* OCDD Project
* Appointment Model class 
*/
	public class AppointmentModel
	{

		public int appointmentID { get; set; }
		public int serviceID { get; set; }
		public int userID { get; set; }
		public DateOnly date { get; set; }
		public TimeOnly time { get; set; }
		public string status { get; set; }








	}
}
