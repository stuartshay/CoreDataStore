using CoreDataStore.Data.Conventions;
using CoreDataStore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreDataStore.Data.SqlServer.Mappings
{
    public class PlutoMapping : IEntityTypeConfiguration<Pluto>
    {
        public void Configure(EntityTypeBuilder<Pluto> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(t => t.BBL).IsRequired();
            builder.Property(t => t.Latitude).HasPrecision(9, 6).IsRequired();
            builder.Property(t => t.Longitude).HasPrecision(9, 6).IsRequired();
            builder.Property(t => t.ZipCode).HasMaxLength(5);
        }
    }
}
