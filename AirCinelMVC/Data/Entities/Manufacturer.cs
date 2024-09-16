using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Manufacturer : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }


        public ICollection<Model> Models { get; set; }


        [Display(Name = "Number of Models")]
        public int NumberModels => Models == null ? 0 : Models.Count;
    }
}
