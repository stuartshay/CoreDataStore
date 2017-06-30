using CoreDataStore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreDataStore.Data.SqlServer.Mappings
{
    public class LandmarkMapping : IEntityTypeConfiguration<Landmark>
    {
        public void Configure(EntityTypeBuilder<Landmark> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(t => t.BoroughID).HasMaxLength(2).IsRequired();
            builder.Property(t => t.BOUNDARIES).HasMaxLength(50).IsRequired();
            builder.Property(t => t.DESIG_ADDR).HasMaxLength(200);
            builder.Property(t => t.HIST_DISTR).HasMaxLength(200);
            builder.Property(t => t.LAST_ACTIO).HasMaxLength(50);
            builder.Property(t => t.LM_NAME).HasMaxLength(200);
            builder.Property(t => t.LP_NUMBER).HasMaxLength(10);
            builder.Property(t => t.LM_TYPE).HasMaxLength(19);
            builder.Property(t => t.NON_BLDG).HasMaxLength(100);
            builder.Property(t => t.OTHER_HEAR).HasMaxLength(200);
            builder.Property(t => t.PLUTO_ADDR).HasMaxLength(200);
            builder.Property(t => t.PUBLIC_HEA).HasMaxLength(200);
            builder.Property(t => t.STATUS).HasMaxLength(50).IsRequired();
            builder.Property(t => t.STATUS_NOT).HasMaxLength(200);

            builder.HasOne(l => l.LPCReport)
                .WithMany(r => r.Landmarks)
                .HasForeignKey(l => l.LP_NUMBER)
                .HasPrincipalKey(r => r.LPNumber);

            builder.HasOne(p => p.Pluto)
                .WithOne(l => l.Landmark)
                .HasForeignKey<Pluto>(p => p.BBL)
                .HasPrincipalKey<Landmark>(l => l.BBL);
        }
    }
}
