using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Models
{
    public class Prescription
    {
        [Key]
        public int IdPrescription { get; set; }
        [Required, DataType("date")]
        public DateTime Date { get; set; }
        [Required, DataType("date")]
        public DateTime DueDate { get; set; }

        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescribedMedicaments { get; set; }
    }
}