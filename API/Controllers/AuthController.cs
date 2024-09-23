//using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using HotelBooking.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IConfiguration _configuration;

        // Constructor to inject the database context and configuration settings (e.g., for JWT token).
        public AuthController(DBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Register a new user with email and password.
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(CustomerDto request)
        {
            // Check if the user already exists in the database.
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("User already exists.");
            }

            // Create password hash and salt for storing the password securely.
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create a new customer object to save in the database.
            var customer = new User
            {
                Email = request.Email,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                //LoginStatus = "Active",  // Set initial login status to "Active".
                //RegisterDate = DateTime.UtcNow  // Store the current time as the registration date.
            };

            // Add the new customer to the database and save changes.
            _context.Users.Add(customer);
            await _context.SaveChangesAsync();

            // Return the registered customer.
            return Ok(customer);
        }

        // Authenticate a user and generate a JWT token if successful.
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(CustomerDto request)
        {
            // Retrieve the customer from the database by email.
            var customer = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (customer == null)
            {
                return BadRequest("User not found.");
            }

            // Verify if the provided password matches the stored password hash.
            if (!VerifyPasswordHash(request.Password, Convert.FromBase64String(customer.PasswordHash), Convert.FromBase64String(customer.PasswordSalt)))
            {
                return BadRequest("Wrong password.");
            }

            // Create a JWT token for the authenticated customer.
            string token = CreateToken(customer);

            // Return the generated token.
            return Ok(token);
        }

        // Create a JWT token for the authenticated customer.
        private string CreateToken(User customer)
        {
            // Define claims for the token (e.g., user ID and email).
            List<Claim> claims = new List<Claim>
            {
                //new Claim(ClaimTypes.NameIdentifier, customer.UserId.ToString()),
                new Claim(ClaimTypes.Email, customer.Email)
            };

            // Get the token key from configuration settings.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            // Specify signing credentials using HMAC SHA512 algorithm.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create the JWT token with the claims and expiration date.
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),  // Token expires in 1 day.
                signingCredentials: creds);

            // Convert the token to a string and return it.
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        // Generate a password hash and salt for secure password storage.
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Use HMACSHA512 algorithm to create the password hash and salt.
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;  // Randomly generated salt.
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));  // Hash the password.
            }
        }

        // Verify if the provided password matches the stored hash using the salt.
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))  // Use the same salt to hash the provided password.
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));  // Compute hash of input password.
                return computedHash.SequenceEqual(storedHash);  // Compare it with the stored hash.
            }
        }
    }

    // Data transfer object (DTO) for customer registration and login requests.
    public class CustomerDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}