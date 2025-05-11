using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        // Foreign Key
        public int FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        public Farmer Farmer { get; set; }
    }
}
