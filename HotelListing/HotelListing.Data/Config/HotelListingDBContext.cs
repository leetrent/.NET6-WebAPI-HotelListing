using HotelListing.Config;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data.Config
{
    public class HotelListingDBContext : DbContext
    {
        public HotelListingDBContext(DbContextOptions<HotelListingDBContext> options): base(options) {}
  
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured == false)
        //    {
        //        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=HotelListingApiDb; Trusted_Connection=True; MultipleActiveResultSets=True;");
        //        optionsBuilder.EnableSensitiveDataLogging();
        //    }
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.ApplyConfiguration(new CountryConfig());
        //    modelBuilder.ApplyConfiguration(new HotelConfig());
        //}
    }
}
