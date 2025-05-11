using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string ContactNumber { get; set; }

        public string FarmLocation { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; }
    }
}
