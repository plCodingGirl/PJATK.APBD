using System;
using System.ComponentModel.DataAnnotations;

namespace CW2.Models
{
    public class UpdateStudentDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}