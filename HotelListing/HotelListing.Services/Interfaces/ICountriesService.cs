﻿using HotelListing.Data;

namespace HotelListing.Services.Interfaces
{
    public interface ICountriesService
    {
        Task<Country> Create(Country country);
        Task<Country> RetrieveById(int id);
        Task<List<Country>> RetrieveAll();
        Task<Country> Update(Country country);
        Task<int> Delete(int id);
    }
}