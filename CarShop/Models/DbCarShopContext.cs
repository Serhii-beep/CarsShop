using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CarShop
{
    public partial class DbCarShopContext : DbContext
    {
        public DbCarShopContext()
        {
        }

        public DbCarShopContext(DbContextOptions<DbCarShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-IO4EGCU;Database=DbCarShop;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PhotoUrl)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Categories");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Cars_Orders");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Producers");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CustomerFullName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Info).IsRequired();

                entity.Property(e => e.LogoUrl)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
