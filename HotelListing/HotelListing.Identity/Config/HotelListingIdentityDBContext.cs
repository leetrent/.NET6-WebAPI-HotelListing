using HotelListing.Identity.Config;
using HotelListing.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data.Config
{
    public class HotelListingIdentityDBContext : IdentityDbContext<ApiUser>
    {
        public HotelListingIdentityDBContext(DbContextOptions<HotelListingIdentityDBContext> options) : base(options) {}

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfig());
        }
    }
}
