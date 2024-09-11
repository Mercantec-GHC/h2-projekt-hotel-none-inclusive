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

        // Constructor to initialize HttpClient
        public DatabaseServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Adds a user to the database.
        public async Task AddUserToDatabaseAsync(UserPostAndPutDTO user)
        {
            // Sends a POST request to add a new user
            await _httpClient.PostAsJsonAsync<UserPostAndPutDTO>(_baseURL + "Users", user);
        }

        // Retrieves all users from the database.
        public async Task<List<UserGetDTO>> GetAllUsers()
        {
            // Sends a GET request to retrieve all users
            return await _httpClient.GetFromJsonAsync<List<UserGetDTO>>(_baseURL + "Users");
        }

        // Gets user info for login.
        public async Task<UserLoginDTO> GetUserToLoginAsync(string email, string password)
        {
            // Sends a GET request to retrieve user information for login
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

        // Retrieves all rooms from the database.
        public async Task<List<GetRoomDTO>> GetRooms()
        {
            // Sends a GET request to retrieve all rooms
            return await _httpClient.GetFromJsonAsync<List<GetRoomDTO>>(_baseURL + "Rooms");
        }

        // Retrieves a specific room by ID from the database.
        public async Task<GetRoomDTO> GetRoomById(int id)
        {
            // Sends a GET request to retrieve a room by its ID
            return await _httpClient.GetFromJsonAsync<GetRoomDTO>(_baseURL + $"Rooms/{id}");
        }

        // Retrieves a user by email from the database.
        public async Task<UserGetDTO> GetUserByEmail(string email)
        {
            // Sends a GET request to retrieve a user by their email
            return await _httpClient.GetFromJsonAsync<UserGetDTO>(_baseURL + $"Users/email/{email}");
        }

        // Creates a new booking in the database.
        public async Task CreateBooking(CreateBookingDTO booking)
        {
            // Sends a POST request to create a new booking
            await _httpClient.PostAsJsonAsync<CreateBookingDTO>(_baseURL + "Booking", booking);
        }
    }
}
