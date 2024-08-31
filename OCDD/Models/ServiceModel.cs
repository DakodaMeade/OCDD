using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OCDD.Models
{
/*
* Dakoda Meade
* OCDD Project
* Service Model class 
* Represents a service
*/
	public class ServiceModel
	{

		public int serviceID { get; set; }

        [DisplayName("Service Name")]
        [Required(ErrorMessage = "Service Name is required.")]
        [StringLength(40, ErrorMessage = "Service Name cannot exceed 40 characters.")]
        public string name { get; set; }

        [DisplayName("Service Description")]
        [Required(ErrorMessage = "Service Description is required.")]
        [StringLength(200, ErrorMessage = "Service Description cannot exceed 200 characters.")]
        public string description { get; set; }

        [DisplayName("Service Price")]
        [Required(ErrorMessage = "Service Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Service Price must be a positive number.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Service Price must be a decimal with up to 2 decimal places.")]
        public decimal price { get; set; }

        [DisplayName("Estimated Duration")]
        [Required(ErrorMessage = "Estimated Duration is required.")]
        [RegularExpression(@"^\d{2}:\d{2}:\d{2}$", ErrorMessage = "Estimated Duration must be in the format HH:MM:SS.")]
        public TimeSpan duration { get; set; }





    }
}
