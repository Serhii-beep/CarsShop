using CarShop.DOM.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.DAL.Data.Configuration
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(e => e.CustomerFullName)
                    .IsRequired()
                    .HasMaxLength(50);
            builder.HasOne(d => d.Warehouse)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Warehouses");
        }
    }
}
