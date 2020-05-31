using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("Patient")]
        public int IdPatient { get; set; }
        [ForeignKey("Doctor")]
        public int IdDoctor { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescribedMedicaments { get; set; }
    }
}