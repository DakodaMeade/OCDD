﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OCDD.Models
{
/*
 * Dakoda Meade
 * OCDD Project
 * User Model class 
 */
	public class UserModel
	{
        // Properties 
        
        public int userID { get; set; }

        [Required]
        [DisplayName("Name")]
        [StringLength(20, MinimumLength = 4)]
        public string name { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public int phoneNumber { get; set; }

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

        [Required]
        [DisplayName("Password")]
        [StringLength(20, MinimumLength = 6)]
        public string password { get; set; }

		public string role { get; set; }

		public List<ServiceModel> serviceHistory { get; set; }




		public void ViewServiceHistory() 
		{

		}

		/*
		// to string method
		public override string ToString()
		{
			return "Username = " + username + " Password = " + password;
		}
		*/
	}
}
