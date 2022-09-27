using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Data.Repositories.Interfaces
{
    public interface ICountriesRepository
    {
        Task<Country> Create(Country country);
        Task<Country> RetrieveById(int id);
        Task<List<Country>> RetrieveAll();
    }
}
