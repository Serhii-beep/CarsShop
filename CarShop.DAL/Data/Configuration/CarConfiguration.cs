using Microsoft.EntityFrameworkCore;
using CarShop.DOM.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.DAL.Data.Configuration
{
    class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasQueryFilter(b => b.OrderId == null);
            builder.Property(e => e.Description).IsRequired();

            builder.Property(e => e.Model)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.PhotoUrl)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(d => d.Category)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_Categories");

            builder.HasOne(d => d.Order)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Cars_Orders");

            builder.HasOne(d => d.Producer)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.ProducerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_Producers");

            builder.HasOne(d => d.Warehouse)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_Warehouses");
        }
    }
}
