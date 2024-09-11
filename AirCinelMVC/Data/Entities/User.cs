﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string LastName { get; set; }


        [MaxLength(100, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Address { get; set; }


        public int CityId { get; set; } 

        public City City { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

    }
}
