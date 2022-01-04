using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarShop.DOM.Database;

namespace CarShop.DAL.Data.Configuration
{
    class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(70);
        }
    }
}
