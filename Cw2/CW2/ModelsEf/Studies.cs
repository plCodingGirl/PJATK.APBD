using System.Collections.Generic;

namespace CW2.ModelsEf
{
    public class Studies
    {
        public Studies()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int IdStudy { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
