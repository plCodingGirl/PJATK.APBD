using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Pharmacy.Models
{
    public class PharmacyDbContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public PharmacyDbContext()
        {
        }

        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasKey(x => new {x.IdMedicament, x.IdPrescription});
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .HasData(
                    new Doctor { IdDoctor = 1, FirstName = "Gregory", LastName = "House", Email = "gregory@house.md"},
                    new Doctor { IdDoctor = 2, FirstName = "Chris", LastName = "Taub", Email = "chris@house.md"},
                    new Doctor { IdDoctor = 3, FirstName = "Remy", LastName = "Hadley", Email = "remy@house.md"});
            modelBuilder.Entity<Patient>()
                .HasData(
                    new Patient { IdPatient = 1, FirstName = "Idina", LastName = "Menzel", BirthDate = new DateTime(1989, 04, 09)},
                    new Patient { IdPatient = 2, FirstName = "Kristen", LastName = "Bell", BirthDate = new DateTime(1990, 03, 05)},
                    new Patient { IdPatient = 3, FirstName = "Josh", LastName = "Gad", BirthDate = new DateTime(1987, 02, 25)});
            modelBuilder.Entity<Medicament>()
                .HasData(
                    new Medicament
                    {
                        IdMedicament = 1, Name = "Cat", Type = "Normal", Description = "Grants vision in total darkness"
                    },
                    new Medicament
                    {
                        IdMedicament = 2, Name = "Tawny Owl", Type = "Normal", Description = "Significantly increases Endurance regeneration."
                    },
                    new Medicament
                    {
                        IdMedicament = 3, Name = "Potion for Triss", Type = "Other", Description = "Alchemical base used to make other potions."
                    });

            modelBuilder.Entity<Prescription>()
                .HasData(new Prescription()
                {
                    IdPrescription = 1, IdDoctor = 1, IdPatient = 2, Date = new DateTime(2020, 05, 31), DueDate = new DateTime(2020, 06, 30)
                });

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasData(new PrescriptionMedicament()
                {
                    IdPrescription = 1,
                    IdMedicament = 3,
                    Details = "Every morning",
                    Dose = 2
                });
        }

        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<PrescriptionMedicament> PrescribedMedicaments { get; set; }
        public virtual DbSet<Medicament> Medicament { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
