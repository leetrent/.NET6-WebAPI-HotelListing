using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Data.Repositories.Interfaces
{
    public interface ICountriesRepository
    {
        Task<List<Country>> RetrieveAll();
        Task<Country> RetrieveById(int id);
        Task<int> Create(Country country);
        Task<int> Update(Country country);
        Task<int> Delete(int id);
    }
}
