using FullStackAssessment_U3Infotech.Models;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssessment_U3Infotech.Data
{
    public class AssessmentDbContext : DbContext
    {
        public AssessmentDbContext(DbContextOptions<AssessmentDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Cafe> Cafes { get; set; }
        public DbSet<EmployeeCafeRelation> EmployeeCafeRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeCafeRelation>()
            .HasIndex(ecr => ecr.EmployeeID)
            .IsUnique();
        }
    }
}
