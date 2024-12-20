﻿using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Models
{
    public class AdminRegisterNewUserViewModel : UserViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
    }
}