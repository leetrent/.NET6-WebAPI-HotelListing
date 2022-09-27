using HotelListing.Data.Config;
using HotelListing.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly HotelListingDBContext _dbContext;

        public CountriesRepository()
        {
            _dbContext = new HotelListingDBContext();
        }
        public CountriesRepository(HotelListingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country> Create(Country country)
        {
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        public async Task<Country> RetrieveById(int id)
        {
            return await _dbContext.Countries.FindAsync(id);
        }

        public async Task<List<Country>> RetrieveAll()
        {
            return await _dbContext.Countries.AsNoTracking().ToListAsync();
        }
    }
}
