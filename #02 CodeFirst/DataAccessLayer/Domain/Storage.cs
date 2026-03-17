using Microsoft.EntityFrameworkCore;
using Model;

namespace Domain;

// todo : 3.0 Add storage class for database context 
public class Storage : DbContext
{
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Disease> Diseases { get; set; } = null!;
    public DbSet<Diagnosis> Diagnoses { get; set; } = null!;

    public Storage() {  }

    public Storage(DbContextOptions<Storage> options) : base(options) { }

    // todo : 3.1 Configure database connection to use SQLite
    // with a file named "hospital.db" in a "Data" directory relative to the project root
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string? parentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            string dataDirectory = Path.Combine(parentDirectory ?? "", "Data");

            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            string dbPath = Path.Combine(dataDirectory, "hospital.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    // todo : 3.2 Configure model relationships and constraints using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Diagnosis>()
            .HasKey(d => new { d.PatientId, d.DiseaseId, d.Completion });

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.Patient)
            .WithMany(p => p.Diagnoses)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.Disease)
            .WithMany(di => di.Diagnoses)
            .HasForeignKey(d => d.DiseaseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Disease>()
            .HasOne(d => d.ParentDisease)
            .WithMany(pd => pd.SubDiseases)
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
