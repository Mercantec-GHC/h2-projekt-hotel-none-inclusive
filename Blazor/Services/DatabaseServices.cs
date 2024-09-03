﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using API.Models;
using System.Net.Http;
using System.Net.Http.Json;


namespace Service
{
    public class DatabaseServices
    {
        private readonly HttpClient _httpClient;

        public DatabaseServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Adds a user to the database.
        public async Task AddUserToDatabaseAsync(string firstName, string lastName, string email, string password, string passwordrepeat, string address, string phonenumber, string city, string country, string zip)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                PasswordRepeat = passwordrepeat,
                Address = address,
                PhoneNumber = phonenumber,
                City = city,
                Country = country,
                Zip = zip
            };
            await _httpClient.PostAsJsonAsync<User>("https://localhost:7207/api/Users", user);

        }
    }
}
