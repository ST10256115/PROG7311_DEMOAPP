using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
