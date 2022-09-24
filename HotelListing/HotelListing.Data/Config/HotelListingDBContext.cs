using HotelListing.Config;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data.Config
{
    internal class HotelListingDBContext : DbContext
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // NOT NEEDED (SO FAR)
        //internal HotelListingDBContext(DbContextOptions<HotelListingDBContext> options): base(options) {}
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal DbSet<Country> Countries { get; set; }
        internal DbSet<Hotel> Hotels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=HotelListingApiDb; Trusted_Connection=True; MultipleActiveResultSets=True;");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new HotelConfig());
        }
    }
}
