using System;
using System.Collections.Generic;

namespace CW2.ModelsEf
{
    public class Enrollment
    {
        public Enrollment()
        {
            Students = new HashSet<Student>();
        }

        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }

        public virtual Studies Studies { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
