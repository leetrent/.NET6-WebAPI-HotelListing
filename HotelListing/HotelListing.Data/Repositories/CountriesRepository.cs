using HotelListing.Data.Config;
using HotelListing.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly HotelListingDBContext _dbContext;

        public CountriesRepository(HotelListingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Country>> RetrieveAll()
        {
            return await _dbContext.Countries.AsNoTracking().Include(c => c.Hotels).ToListAsync();
        }

        public async Task<Country> RetrieveById(int id)
        {
            return await _dbContext.Countries.AsNoTracking().Where(c => c.Id == id).Include(c => c.Hotels).FirstOrDefaultAsync();
        }

        public async Task<int> Create(Country country)
        {
            _dbContext.Countries.Add(country);
            int rowsCreated = await _dbContext.SaveChangesAsync();

            Console.WriteLine($"[CountriesRepository][Create] => (rowsCreated): '{rowsCreated}'");

            return country.Id;
        }

        public async Task<int> Update(Country country)
        {
            if (await this.CountryExists(country.Id) == true)
            {
                _dbContext.Entry(country).State = EntityState.Modified;
                int rowsUpdated = await _dbContext.SaveChangesAsync();

                Console.WriteLine($"[CountriesRepository][Update] => (rowsUpdated): '{rowsUpdated}'");

                return country.Id;
            }
            else
            {
                throw new Exception($"Country with an ID of '{country.Id}' was not found. Cannot update.");
            }
        }

        public async Task<int> Delete(int id)
        {
            if (await this.CountryExists(id) == true)
            {
                var country = await _dbContext.Countries.FindAsync(id);
                _dbContext.Countries.Remove(country);
                return await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Country with an ID of '{id}' was not found. Cannot delete.");
            }
        }

        public async Task<bool> CountryExists(int id)
        {
            return await _dbContext.Countries.AnyAsync(e => e.Id == id);
        }
    }
}
