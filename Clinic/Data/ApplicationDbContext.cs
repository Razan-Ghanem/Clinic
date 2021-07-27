using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data
{
    public class ApplicationDbContext:DbContext 
{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // call when update database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Specialization>().HasData(
                new { Id = (long)1, SpecializationName = "Anesthesiologist" },
                new { Id = (long)2, SpecializationName = "Cardiologists" },
                new { Id = (long)3, SpecializationName = "Hematologist" },
                new { Id = (long)4, SpecializationName = "Radiologist" }) ;

            modelBuilder.Entity<Patient>().HasIndex(p => p.SSN).IsUnique();
                
        }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
        
    public DbSet<Specialization> Specializations { get; set; }

    public DbSet<AppType> AppTypes { get; set; }
    public DbSet<MedicalHistory> MedicalHistories { get; set; }

    }
}
