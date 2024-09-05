using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Airport : IEntity
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Code { get; set; }


        public int CityID { get; set; }

        public City City { get; set; }

    }
}
