using CoreDataStore.Data.Conventions;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace CoreDataStore.Data.SqlServer
{
    public class NYCLandmarkContext : DbContext
    {
        public NYCLandmarkContext(DbContextOptions<NYCLandmarkContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.RemovePluralizingTableNameConvention();

            builder.Entity<LPCReport>().HasKey(m => m.Id);
            builder.Entity<LPCReport>().Property(t => t.Name).HasMaxLength(200).IsRequired();
            builder.Entity<LPCReport>().Property(t => t.Architect).HasMaxLength(200);
            builder.Entity<LPCReport>().Property(t => t.Borough).HasMaxLength(20);
            builder.Entity<LPCReport>().Property(t => t.ObjectType).HasMaxLength(50);
            builder.Entity<LPCReport>().Property(t => t.LPNumber).HasMaxLength(10).IsRequired();
            builder.Entity<LPCReport>().Property(t => t.LPCId).HasMaxLength(10).IsRequired();
            builder.Entity<LPCReport>().Property(t => t.PhotoURL).HasMaxLength(500);
            builder.Entity<LPCReport>().Property(t => t.Style).HasMaxLength(100);

            //Shadow Properties
            //builder.Entity<LPCReport>().Property<DateTime>("Modified");

            builder.Entity<Landmark>().HasKey(m => m.Id);
            builder.Entity<Landmark>().Property(t => t.BoroughID).HasMaxLength(2).IsRequired();
            builder.Entity<Landmark>().Property(t => t.BOUNDARIES).HasMaxLength(50).IsRequired();
            builder.Entity<Landmark>().Property(t => t.DESIG_ADDR).HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.HIST_DISTR).HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.LAST_ACTIO).HasMaxLength(50);
            builder.Entity<Landmark>().Property(t => t.LM_NAME).HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.LP_NUMBER).HasMaxLength(10);
            builder.Entity<Landmark>().Property(t => t.LM_TYPE).HasMaxLength(19);
            builder.Entity<Landmark>().Property(t => t.NON_BLDG).HasMaxLength(100);
            builder.Entity<Landmark>().Property(t => t.OTHER_HEAR).HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.PLUTO_ADDR).HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.PUBLIC_HEA).HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.STATUS).HasMaxLength(50).IsRequired();
            builder.Entity<Landmark>().Property(t => t.STATUS_NOT).HasMaxLength(200);

            builder.Entity<Landmark>()
                .HasOne(l => l.LPCReport)
                .WithMany(r => r.Landmarks)
                .HasForeignKey(l => l.LP_NUMBER)
                .HasPrincipalKey(r => r.LPNumber);


            builder.Entity<Landmark>()
                .HasOne(p => p.Pluto)
                .WithOne(l => l.Landmark)
                .HasForeignKey<Pluto>(p => p.BBL)
                .HasPrincipalKey<Landmark>(l => l.BBL);
                  
            builder.Entity<Pluto>().HasKey(m => m.Id);
            builder.Entity<Pluto>().Property(t => t.BBL).IsRequired();
            builder.Entity<Pluto>().Property(t => t.Latitude).HasPrecision(9, 6).IsRequired();
            builder.Entity<Pluto>().Property(t => t.Longitude).HasPrecision(9, 6).IsRequired();
            builder.Entity<Pluto>().Property(t => t.ZipCode).HasMaxLength(5);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var modifiedBidEntries = ChangeTracker.Entries<IAuditableEntity>()
                 .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (EntityEntry<IAuditableEntity> entity in modifiedBidEntries)
            {
               //entity.Property("Modified").CurrentValue = DateTime.UtcNow;
               //entity.Property("ObjectType").CurrentValue = "XXXXXXXX";
            }

            base.SaveChanges();
            return 0;
        }

        public DbSet<LPCReport> LPCReports { get; set; }

        public DbSet<Landmark> Landmarks { get; set; }

        public DbSet<Pluto> Pluto { get; set; }
    }
}
