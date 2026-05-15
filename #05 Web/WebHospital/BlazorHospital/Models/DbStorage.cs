using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorHospital.Models;

public partial class DbStorage : DbContext
{
    public DbStorage()
    {
    }

    public DbStorage(DbContextOptions<DbStorage> options)
        : base(options)
    {
    }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<EfmigrationsLock> EfmigrationsLocks { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=..\\Data\\hospital.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => new { e.PatientId, e.DiseaseId, e.Completion });

            entity.HasIndex(e => e.DiseaseId, "IX_Diagnoses_DiseaseId");

            entity.HasOne(d => d.Disease).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.DiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Patient).WithMany(p => p.Diagnoses).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasIndex(e => e.ParentId, "IX_Diseases_ParentId");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EfmigrationsLock>(entity =>
        {
            entity.ToTable("__EFMigrationsLock");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
