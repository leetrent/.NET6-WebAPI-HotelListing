using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Services.DTOs.Country
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string ShortName { get; set; } = String.Empty;
    }
}
