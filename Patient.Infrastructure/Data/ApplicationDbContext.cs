using Microsoft.EntityFrameworkCore;
using Patient.Domain.Entities;
using System.Collections.Generic;

namespace Patient.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para a entidade Patient.
        public DbSet<Patient.Domain.Entities.Patient> Patients { get; set; }

        // DbSet para a entidade MedicalHistory.
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
    }
}
