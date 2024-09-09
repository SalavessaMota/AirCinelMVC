﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Airplane : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Model { get; set; }


        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Manufacturer { get; set; }


        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public int Capacity { get; set; }


        [Display(Name = "Year of Manufacture")]
        public int YearOfManufacture { get; set; }


        public User User { get; set; }


        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                return $"https://localhost:44334{ImageUrl.Substring(1)}";
            }
        }

    }
}
