using System.Threading.Tasks;
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
        private readonly string _baseURL = "https://localhost:7207/api/";

        public DatabaseServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Adds a user to the database.
        public async Task AddUserToDatabaseAsync(string firstName, string lastName, string email, string password, string passwordrepeat, string address, string phonenumber, string city, string country, string zip)
        {
            var user = new UserPostAndPutDTO
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

            await _httpClient.PostAsJsonAsync<UserPostAndPutDTO>(_baseURL + "Users", user);
        }

        // Retrieves all users from the database.
        public async Task<List<UserGetDTO>> GetAllUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserGetDTO>>(_baseURL + "Users");
        }

        // Gets user info for login.
        public async Task<UserLoginDTO> GetUserToLoginAsync(string email, string password)
        {
            var response = await _httpClient.GetAsync($"{_baseURL}Users/{email}/{password}");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to UserLoginDTO
                var user = await response.Content.ReadFromJsonAsync<UserLoginDTO>();
                return user;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Handle login failure (e.g., user not found or incorrect password)
                return null; // or throw a custom exception
            }
            else
            {
                // Handle other potential errors (e.g., server errors)
                response.EnsureSuccessStatusCode();
                return null; // This line won't be reached, but it's necessary to satisfy the compiler
            }
        }

        public async Task<List<GetRoomDTO>> GetRooms()
        {
            return await _httpClient.GetFromJsonAsync<List<GetRoomDTO>>(_baseURL + "Rooms");
        }


    }
}
