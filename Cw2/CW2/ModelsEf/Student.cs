using System;

namespace CW2.ModelsEf
{
    public class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public virtual Enrollment Enrollment { get; set; }
    }
}
