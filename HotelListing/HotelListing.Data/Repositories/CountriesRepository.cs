using HotelListing.Data.Config;
using HotelListing.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly HotelListingDBContext _dbContext;

        //public CountriesRepository()
        //{
        //    _dbContext = new HotelListingDBContext();
        //}
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
            return await _dbContext.Countries.AsNoTracking().Where( c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Country>> RetrieveAll()
        {
            return await _dbContext.Countries.AsNoTracking().ToListAsync();
        }

        public async Task<Country> Update(Country country)
        {
            if (await this.CountryExists(country.Id) == true)
            {
                _dbContext.Entry(country).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return await this.RetrieveById(country.Id);
            }
            else
            {
                throw new Exception($"Country with an ID of '{country.Id}' was not found. Cannot update.");
            }
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _dbContext.Countries.AnyAsync(e => e.Id == id);
        }
    }
}
