using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DLADbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<SoftwareLicense> SoftwareLicenses { get; set; }
        public DbSet<InstalledSoftware> InstalledSoftwares { get; set; }
        public DbSet<EquipmentMovement> EquipmentMovements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EADB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Manager).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Employee>(e =>
            {
                e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
                e.Property(x => x.Position).HasMaxLength(100);

                e.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EquipmentType>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Equipment>(e =>
            {
                e.Property(x => x.InventoryNumber).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.InventoryNumber).IsUnique();

                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.Property(x => x.SerialNumber).HasMaxLength(100);

                e.Property(x => x.Status).HasDefaultValue(0);
                e.Property(x => x.RegisteredAt).HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.EquipmentType)
                    .WithMany()
                    .HasForeignKey(x => x.EquipmentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Employee)
                    .WithMany()
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<SoftwareLicense>(e =>
            {
                e.Property(x => x.SoftwareName).HasMaxLength(150).IsRequired();
                e.Property(x => x.Manufacturer).HasMaxLength(100).IsRequired();
                e.Property(x => x.LicenseKey).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<InstalledSoftware>(e =>
            {
                e.HasKey(x => new { x.EquipmentId, x.SoftwareLicenseId });

                e.HasOne(x => x.Equipment)
                    .WithMany()
                    .HasForeignKey(x => x.EquipmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.SoftwareLicense)
                    .WithMany()
                    .HasForeignKey(x => x.SoftwareLicenseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EquipmentMovement>(e =>
            {
                e.HasOne(x => x.Equipment)
                    .WithMany()
                    .HasForeignKey(x => x.EquipmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.OldEmployee)
                    .WithMany()
                    .HasForeignKey(x => x.OldEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.NewEmployee)
                    .WithMany()
                    .HasForeignKey(x => x.NewEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
