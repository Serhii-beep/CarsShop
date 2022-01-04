using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarShop.DOM.Database;

namespace CarShop.DAL.Data.Configuration
{
    class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(30);

            builder.Property(e => e.Info).IsRequired();

            builder.Property(e => e.LogoUrl)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
