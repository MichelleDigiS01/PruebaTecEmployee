using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class PruebaTecnicaContext : DbContext
{
    public PruebaTecnicaContext()
    {
    }

    public PruebaTecnicaContext(DbContextOptions<PruebaTecnicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<DTOs> DTOs { get; set; }
    public virtual DbSet<LoginDTO> LoginDTO { get; set; }
    public virtual DbSet<GenderReport> GenderDTO { get; set; }
    public virtual DbSet<CityReport> CityDTO { get; set; }
    public virtual DbSet<EducationReport> EducationDTO { get; set; }
    public virtual DbSet<AverageAgeReport> AverageDTO { get; set; }
    public virtual DbSet<ExperienceTierReport> ExperienceTierDTO { get; set; }
    public virtual DbSet<EmployeesBenched> EmployeesBenchedDTO { get; set; }
    public virtual DbSet<LeavePrediction> LeavePredictionDTO { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.; Database=PruebaTecnica; TrustServerCertificate=True; User ID=sa; Password=pass@word1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            modelBuilder.Entity<DTOs>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<LoginDTO>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<GenderReport>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<CityReport>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<EducationReport>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<AverageAgeReport>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<ExperienceTierReport>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<EmployeesBenched>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<LeavePrediction>(entity =>
            {
                entity.HasNoKey();
            });

            

            entity.HasKey(e => e.IdCity).HasName("PK__City__394B023A545D2BE9");

            entity.ToTable("City");

            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.IdEducation).HasName("PK__Educatio__5E748C7EE09D88A6");

            entity.ToTable("Education");

            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PK__Employee__51C8DD7A4EED11D5");

            entity.ToTable("Employee");

            entity.Property(e => e.EverBenched)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCityNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__IdCity__182C9B23");

            entity.HasOne(d => d.IdEducationNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdEducation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__IdEduc__1920BF5C");

            entity.HasOne(d => d.IdGenderNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdGender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__IdGend__1A14E395");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.IdGender).HasName("PK__Gender__0042D43B284381D6");

            entity.ToTable("Gender");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Gender");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C3A2F9298");

            entity.ToTable("Rol");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rol");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Usuario__1788CC4C28374309");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.UserName, "UQ__Usuario__C9F28456F7FC68DC").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__IdEmplo__1DE57479");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__IdRol__1ED998B2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
