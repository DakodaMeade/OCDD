using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OCDD.Models
{
	/*
* Dakoda Meade
* OCDD Project
* Appointment Model class 
*/
	public class AppointmentModel
	{
        [DisplayName("Appointment ID")]
        public int appointmentID { get; set; }
		//public int serviceID { get; set; }
		//public int userID { get; set; }
        public ServiceModel service { get; set; }
        public UserModel user { get; set; }

        [DisplayName("Date and Time")]
        public DateTime dateTime { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime date { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public TimeSpan time { get; set; }

        [DisplayName("Vehicle Year")]
        [Required(ErrorMessage = "Vehicle year is required")]
        public int vehicleYear { get; set; }

        [DisplayName("Vehicle Make")]
        [Required(ErrorMessage = "Vehicle make is required")]
        public string vehicleMake { get; set; }

        [DisplayName("Vehicle Model")]
        [Required(ErrorMessage = "Vehicle model is required")]
        public string vehicleModel { get; set; }

        [DisplayName("Appointment Status")]
        public string status { get; set; }

        // informarion for non registered users
        [Required]
        [DisplayName("Name")]
        [StringLength(20, MinimumLength = 4)]
        public string name { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string phoneNumber { get; set; }

        [Required]
        [DisplayName("Address")]
        public string address { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip code must be exactly 5 digits.")]
        public int zipCode { get; set; }

        [Required]
        [DisplayName("City")]
        [StringLength(20, MinimumLength = 4)]
        public string city { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{2}$", ErrorMessage = "State must be 2 characters")]
        [DisplayName("State")]
        public string state { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [DisplayName("Email")]
        public string email { get; set; }


        


    }
}
