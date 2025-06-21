using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VeterinaryClinic.Models;

public partial class VeterinaryClinicContext : DbContext
{
    public VeterinaryClinicContext()
    {
    }

    public VeterinaryClinicContext(DbContextOptions<VeterinaryClinicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Breed> Breeds { get; set; }

    public virtual DbSet<Case> Cases { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientDetail> PatientDetails { get; set; }

    public virtual DbSet<PharmacyInventory> PharmacyInventories { get; set; }

    public virtual DbSet<Prescribedmed> Prescribedmeds { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<Species> Species { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=VeterinaryClinic;UId=sa;pwd=123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Breed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__breeds__3213E83F92EA875C");

            entity.ToTable("breeds");

            entity.HasIndex(e => e.Name, "UQ__breeds__72E12F1BB12087B1").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Case>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cases__3213E83F994D5E97");

            entity.ToTable("cases");

            entity.HasIndex(e => new { e.PatientId, e.Date, e.DoctorVcnNo }, "idx_case_search");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CaseType)
                .HasMaxLength(10)
                .HasColumnName("case_type");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DiagnosisId).HasColumnName("diagnosis_id");
            entity.Property(e => e.DoctorVcnNo)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("doctor_vcn_no");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.PatientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");

            entity.HasOne(d => d.Diagnosis).WithMany(p => p.Cases)
                .HasForeignKey(d => d.DiagnosisId)
                .HasConstraintName("FK__cases__diagnosis__08B54D69");

            entity.HasOne(d => d.DoctorVcnNoNavigation).WithMany(p => p.Cases)
                .HasPrincipalKey(p => p.VcnNo)
                .HasForeignKey(d => d.DoctorVcnNo)
                .HasConstraintName("FK__cases__doctor_vc__09A971A2");

            entity.HasOne(d => d.Patient).WithMany(p => p.Cases)
                .HasPrincipalKey(p => p.PatientId)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__cases__patient_i__07C12930");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__clients__F3DBC57339312F1D");

            entity.ToTable("clients");

            entity.HasIndex(e => e.ClientId, "UQ__clients__BF21A42575338DAC").IsUnique();

            entity.HasIndex(e => e.ClientId, "idx_client_search");

            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Address)
                .HasMaxLength(64)
                .HasColumnName("address");
            entity.Property(e => e.ClientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("client_id");

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.Username)
                .HasConstraintName("FK__clients__usernam__6E01572D");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__diagnosi__3213E83FC8B1150E");

            entity.ToTable("diagnosis");

            entity.HasIndex(e => e.Name, "UQ__diagnosi__72E12F1B74888C70").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__doctors__F3DBC5732A2A4BAE");

            entity.ToTable("doctors");

            entity.HasIndex(e => e.VcnNo, "UQ__doctors__0CAF968A2937F80E").IsUnique();

            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .HasColumnName("email");
            entity.Property(e => e.VcnNo)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("vcn_no");
            entity.Property(e => e.YearGraduated).HasColumnName("year_graduated");

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.Username)
                .HasConstraintName("FK__doctors__usernam__71D1E811");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__patients__3213E83F012F02AD");

            entity.ToTable("patients");

            entity.HasIndex(e => e.PatientId, "UQ__patients__4D5CE4775E3C40C6").IsUnique();

            entity.HasIndex(e => new { e.ClientId, e.PatientId }, "idx_patient_search");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgeGroup)
                .HasMaxLength(10)
                .HasColumnName("age_group");
            entity.Property(e => e.BreedId).HasColumnName("breed_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("client_id");
            entity.Property(e => e.PatientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.Sex)
                .HasMaxLength(6)
                .HasColumnName("sex");
            entity.Property(e => e.SpeciesId).HasColumnName("species_id");

            entity.HasOne(d => d.Breed).WithMany(p => p.Patients)
                .HasForeignKey(d => d.BreedId)
                .HasConstraintName("FK__patients__breed___02FC7413");

            entity.HasOne(d => d.Client).WithMany(p => p.Patients)
                .HasPrincipalKey(p => p.ClientId)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__patients__client__01142BA1");

            entity.HasOne(d => d.Species).WithMany(p => p.Patients)
                .HasForeignKey(d => d.SpeciesId)
                .HasConstraintName("FK__patients__specie__02084FDA");
        });

        modelBuilder.Entity<PatientDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("patient_details");

            entity.Property(e => e.AgeGroup)
                .HasMaxLength(10)
                .HasColumnName("age_group");
            entity.Property(e => e.BreedId).HasColumnName("breed_id");
            entity.Property(e => e.ClientFirstName)
                .HasMaxLength(64)
                .HasColumnName("client_first_name");
            entity.Property(e => e.ClientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("client_id");
            entity.Property(e => e.ClientLastName)
                .HasMaxLength(64)
                .HasColumnName("client_last_name");
            entity.Property(e => e.PatientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.Sex)
                .HasMaxLength(6)
                .HasColumnName("sex");
            entity.Property(e => e.SpeciesId).HasColumnName("species_id");
        });

        modelBuilder.Entity<PharmacyInventory>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__pharmacy__52020FDD6945253A");

            entity.ToTable("pharmacy_inventory");

            entity.HasIndex(e => e.Barcode, "UQ__pharmacy__C16E36F867DAEE2A").IsUnique();

            entity.HasIndex(e => new { e.ItemId, e.TradeName, e.GenericName, e.Barcode }, "idx_inventory_search");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Barcode)
                .HasMaxLength(255)
                .HasColumnName("barcode");
            entity.Property(e => e.Category)
                .HasMaxLength(64)
                .HasColumnName("category");
            entity.Property(e => e.CostPricePerUnit)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost_price_per_unit");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.GenericName)
                .HasMaxLength(255)
                .HasColumnName("generic_name");
            entity.Property(e => e.SalePricePerUnit)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sale_price_per_unit");
            entity.Property(e => e.TradeName)
                .HasMaxLength(255)
                .HasColumnName("trade_name");
            entity.Property(e => e.Unit)
                .HasMaxLength(32)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<Prescribedmed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__prescrib__3213E83F05D4D2D7");

            entity.ToTable("prescribedmeds");

            entity.HasIndex(e => new { e.PrescriptionId, e.MedicationId, e.Date }, "idx_prescription_search");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Frequency)
                .HasMaxLength(32)
                .HasColumnName("frequency");
            entity.Property(e => e.MedicationId).HasColumnName("medication_id");
            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Route)
                .HasMaxLength(32)
                .HasColumnName("route");

            entity.HasOne(d => d.Medication).WithMany(p => p.Prescribedmeds)
                .HasForeignKey(d => d.MedicationId)
                .HasConstraintName("FK__prescribe__medic__14270015");

            entity.HasOne(d => d.Prescription).WithMany(p => p.Prescribedmeds)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK__prescribe__presc__1332DBDC");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__prescrip__3213E83FD6589BA4");

            entity.ToTable("prescriptions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.PatientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.PrescribingDoctorVcnNo)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("prescribing_doctor_vcn_no");

            entity.HasOne(d => d.Patient).WithMany(p => p.Prescriptions)
                .HasPrincipalKey(p => p.PatientId)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__prescript__patie__0F624AF8");

            entity.HasOne(d => d.PrescribingDoctorVcnNoNavigation).WithMany(p => p.Prescriptions)
                .HasPrincipalKey(p => p.VcnNo)
                .HasForeignKey(d => d.PrescribingDoctorVcnNo)
                .HasConstraintName("FK__prescript__presc__10566F31");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__receipts__3213E83F55FC1692");

            entity.ToTable("receipts");

            entity.HasIndex(e => new { e.Date, e.PatientId, e.PaymentMethod, e.PrescriptionId }, "idx_receipt_search");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.PatientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(10)
                .HasColumnName("payment_method");
            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Patient).WithMany(p => p.Receipts)
                .HasPrincipalKey(p => p.PatientId)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__receipts__patien__17F790F9");

            entity.HasOne(d => d.Prescription).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK__receipts__prescr__18EBB532");
        });

        modelBuilder.Entity<Species>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__species__3213E83F6AD15BAB");

            entity.ToTable("species");

            entity.HasIndex(e => e.Name, "UQ__species__72E12F1B4CEB6234").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__users__F3DBC573ECEC2510");

            entity.ToTable("users");

            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.FirstName)
                .HasMaxLength(64)
                .HasColumnName("first_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(64)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone_no");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
