using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Models
{
    public class DoctorDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
    }
}