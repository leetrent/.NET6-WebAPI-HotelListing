using HotelListing.Data.Config;
using HotelListing.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data.Repositories
{
    public class HotelsRepository : IHotelsRepository
    {
        private readonly HotelListingDBContext _dbContext;

        public HotelsRepository(HotelListingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Hotel>> RetrieveAll()
        {
            return await _dbContext.Hotels.AsNoTracking().Include(h => h.Country).ToListAsync();
        }

        public async Task<Hotel> RetrieveById(int id)
        {
            return await _dbContext.Hotels.AsNoTracking().Where(h => h.Id == id).Include(h => h.Country).FirstOrDefaultAsync();
        }

        public async Task<int> Create(Hotel hotel)
        {
            //hotel.Country = await _dbContext.Countries.AsNoTracking().Where(c => c.Id == hotel.CountryId).FirstOrDefaultAsync();
            Console.WriteLine($"(hotel.Id).......: '{hotel.Id}'");
            Console.WriteLine($"(hotel.Name).....: '{hotel.Name}'");
            Console.WriteLine($"(hotel.Address)..: '{hotel.Address}'");
            Console.WriteLine($"(hotel.Rating)...: '{hotel.Rating}'");
            Console.WriteLine($"(hotel.CountryId): '{hotel.CountryId}'");

            _dbContext.Hotels.Add(hotel);
            int rowsCreated = await _dbContext.SaveChangesAsync();

             return hotel.Id;
        }
        public async Task<int> Update(Hotel hotel)
        {
            if (await this.HotelsExists(hotel.Id) == true)
            {
                _dbContext.Entry(hotel).State = EntityState.Modified;
                int rowsUpdated = await _dbContext.SaveChangesAsync();

                return hotel.Id;
            }
            else
            {
                throw new Exception($"Hotel with an ID of '{hotel.Id}' was not found. Cannot update.");
            }
        }

        public async Task<int> Delete(int id)
        {
            if (await this.HotelsExists(id) == true)
            {
                Hotel hotel = await _dbContext.Hotels.FindAsync(id);
                _dbContext.Hotels.Remove(hotel);
                return await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Country with an ID of '{id}' was not found. Cannot delete.");
            }
        }

        private async Task<bool> HotelsExists(int id)
        {
            return await _dbContext.Hotels.AnyAsync(e => e.Id == id);
        }
    }
}
