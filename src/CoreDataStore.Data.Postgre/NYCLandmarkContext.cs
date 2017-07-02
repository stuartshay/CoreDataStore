using Microsoft.EntityFrameworkCore;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Data.Conventions;

namespace CoreDataStore.Data.Postgre
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
            builder.Entity<LPCReport>().Property(t => t.Architect).HasColumnType("varchar").HasMaxLength(200);  
            builder.Entity<LPCReport>().Property(t => t.Borough).HasColumnType("varchar").HasMaxLength(20);
            builder.Entity<LPCReport>().Property(t => t.ObjectType).HasColumnType("varchar").HasMaxLength(50);
            builder.Entity<LPCReport>().Property(t => t.LPNumber).HasColumnType("varchar").IsRequired().HasMaxLength(10);
            builder.Entity<LPCReport>().Property(t => t.LPCId).HasColumnType("varchar").IsRequired().HasMaxLength(10);
            builder.Entity<LPCReport>().Property(t => t.Name).HasColumnType("varchar").IsRequired(); ;
            builder.Entity<LPCReport>().Property(t => t.PhotoURL).HasColumnType("varchar").HasMaxLength(500);
            builder.Entity<LPCReport>().Property(t => t.Style).HasColumnType("varchar").HasMaxLength(100);
            builder.Entity<LPCReport>().Property(t => t.DateDesignated); //.HasColumnType("NpgsqlDate");


            builder.Entity<LPCLocation>().HasKey(m => m.Id);
            builder.Entity<LPCLocation>().Property(t => t.Name).HasColumnType("varchar").HasMaxLength(200).IsRequired();
            builder.Entity<LPCLocation>().Property(t => t.LPNumber).HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Entity<LPCLocation>().Property(t => t.Borough).HasColumnType("varchar").HasMaxLength(13);
            builder.Entity<LPCLocation>().Property(t => t.ZipCode).HasColumnType("varchar").HasMaxLength(5);
            builder.Entity<LPCLocation>().Property(t => t.ObjectType).HasColumnType("varchar").HasMaxLength(50);
            builder.Entity<LPCLocation>().Property(t => t.Latitude).HasPrecision(9, 6);
            builder.Entity<LPCLocation>().Property(t => t.Longitude).HasPrecision(9, 6);


            builder.Entity<LPCLamppost>().HasKey(m => m.Id);
            builder.Entity<LPCLamppost>().Property(t => t.Type).HasColumnType("varchar").HasMaxLength(50);
            builder.Entity<LPCLamppost>().Property(t => t.SubType).HasColumnType("varchar").HasMaxLength(50);
            builder.Entity<LPCLamppost>().Property(t => t.Borough).HasColumnType("varchar").HasMaxLength(20);
            builder.Entity<LPCLamppost>().Property(t => t.Latitude).HasPrecision(9, 6);
            builder.Entity<LPCLamppost>().Property(t => t.Longitude).HasPrecision(9, 6);


            builder.Entity<Landmark>().HasKey(m => m.Id);
            builder.Entity<Landmark>().Property(t => t.BoroughID).HasColumnType("varchar").HasMaxLength(2).IsRequired();
            builder.Entity<Landmark>().Property(t => t.BOUNDARIES).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Entity<Landmark>().Property(t => t.DESIG_ADDR).HasColumnType("varchar").HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.HIST_DISTR).HasColumnType("varchar").HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.LAST_ACTIO).HasColumnType("varchar").HasMaxLength(50);
            builder.Entity<Landmark>().Property(t => t.LM_NAME).HasColumnType("varchar").HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.LP_NUMBER).HasColumnType("varchar").HasMaxLength(10);
            builder.Entity<Landmark>().Property(t => t.LM_TYPE).HasColumnType("varchar").HasMaxLength(19);
            builder.Entity<Landmark>().Property(t => t.NON_BLDG).HasColumnType("varchar").HasMaxLength(100);
            builder.Entity<Landmark>().Property(t => t.OTHER_HEAR).HasColumnType("varchar").HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.PLUTO_ADDR).HasColumnType("varchar").HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.PUBLIC_HEA).HasColumnType("varchar").HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.STATUS).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Entity<Landmark>().Property(t => t.STATUS_NOT).HasColumnType("varchar").HasMaxLength(200);
            builder.Entity<Landmark>().Property(t => t.CALEN_DATE).HasColumnType("date");
            builder.Entity<Landmark>().Property(t => t.DESIG_DATE).HasColumnType("date");

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

            builder.Entity<Pluto>().ToTable("PLUTO");
            builder.Entity<Pluto>().HasKey(m => m.Id);
            builder.Entity<Pluto>().Property(t => t.Id).HasColumnName("id");
            builder.Entity<Pluto>().Property(t => t.BBL).IsRequired();
            builder.Entity<Pluto>().Property(t => t.Address).HasMaxLength(28);
            builder.Entity<Pluto>().Property(t => t.ZipCode).HasMaxLength(5);
            builder.Entity<Pluto>().Property(t => t.Latitude).HasPrecision(9, 6).IsRequired();
            builder.Entity<Pluto>().Property(t => t.Longitude).HasPrecision(9, 6).IsRequired();

            base.OnModelCreating(builder);
        }

        public DbSet<LPCReport> LPCReports { get; set; }

        public DbSet<LPCLocation> LPCLocation { get; set; }

        public DbSet<LPCLamppost> LPCLamppost { get; set; }

        public DbSet<Landmark> Landmarks { get; set; }

        public DbSet<Pluto> Pluto { get; set; }

    }
}
