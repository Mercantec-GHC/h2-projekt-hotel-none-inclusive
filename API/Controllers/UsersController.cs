using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using API.Models;
using API.Services;

namespace API.Controllers
{
    // Defines the route for the API and marks the controller as an API controller
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Database context and user mapping service
        private readonly DBContext _context;
        private readonly UserMapping _userMapping;

        // Constructor to inject DBContext and UserMapping services
        public UsersController(DBContext context, UserMapping userMapping) 
        {
            _context = context;
            _userMapping = userMapping;
        }

        #region GetUsers
        // GET: api/Users
        // Retrieves all users from the database and maps them to UserGetDTO
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGetDTO>>> GetUsers()
        {
            var users = await _context.Users.Select(user => new UserGetDTO
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                Country = user.Country,
                Zip = user.Zip,
               
                
            }).ToListAsync();

            return Ok(users); // Returns the list of users
        }
        #endregion

        #region Login
        // POST: /login
        // Authenticates a user by email and password for login
        [HttpPost("/login")]
        public async Task<ActionResult<UserLoginDTO>> Login(string email, string password)
        {
            // Find the user by email and password
            var user = await _context.Users.Where(e => e.Email == email).FirstOrDefaultAsync(p => p.Password == password);

            // If user not found, return NotFound
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user); // Returns the authenticated user
        }
        #endregion

        #region GetUserWithID
        // GET: api/Users/id/{id}
        // Retrieves a user by their ID
        [HttpGet("id/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            // If user is not found, return NotFound
            if (user == null)
            {
                return NotFound();
            }

            return user; // Returns the found user
        }
        #endregion

        #region GetAUserByEmail
        // GET: api/Users/email/{email}
        // Retrieves a user by their email address
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserGetDTO>> GetAUserByEmail(string email)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            // If user is not found, return NotFound
            if (user == null)
            {
                return NotFound();
            }

            // Map the user to UserGetDTO and return
            return _userMapping.MapUserToUserGetDTO(user);
        }
        #endregion

        #region PutUser
        // PUT: api/Users/{id}
        // Updates an existing user by their ID using data from UserPostAndPutDTO
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserPostAndPutDTO UserDTO)
        {
            // Find the user in the database by their ID
            var user = await _context.Users.FindAsync(id);

            // If the user does not exist, return NotFound
            if (user == null)
            {
                return NotFound();
            }

            // Update the user fields from the DTO, throwing exception if a required field is missing
            user.FirstName = UserDTO.FirstName ?? throw new ArgumentNullException(nameof(UserDTO.FirstName));
            user.LastName = UserDTO.LastName ?? throw new ArgumentNullException(nameof(UserDTO.LastName));
            user.Email = UserDTO.Email ?? throw new ArgumentNullException(nameof(UserDTO.Email));
            user.Password = UserDTO.Password ?? throw new ArgumentNullException(nameof(UserDTO.Password));
            user.Address = UserDTO.Address ?? throw new ArgumentNullException(nameof(UserDTO.Address));
            user.PhoneNumber = UserDTO.PhoneNumber ?? throw new ArgumentNullException(nameof(UserDTO.PhoneNumber));
            user.City = UserDTO.City ?? throw new ArgumentNullException(nameof(UserDTO.City));
            user.Country = UserDTO.Country ?? throw new ArgumentNullException(nameof(UserDTO.Country));
            user.Zip = UserDTO.Zip ?? throw new ArgumentNullException(nameof(UserDTO.Zip));

            // Mark the user as modified in the database context
            _context.Entry(user).State = EntityState.Modified;

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If the user no longer exists, return NotFound
                if (!_context.Users.Any(u => u.UserId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Re-throw exception if any other issue occurs
                }
            }

            return NoContent(); // Return NoContent on successful update
        }
        #endregion

        #region PostUser
        // POST: api/Users
        // Creates a new user from the UserPostAndPutDTO
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserPostAndPutDTO UserDTO)
        {
            try
            {
                // Create a new user from the DTO data
                User user = new User()
                {
                    FirstName = UserDTO.FirstName,
                    LastName = UserDTO.LastName,
                    Email = UserDTO.Email,
                    Password = UserDTO.Password,
                    PasswordRepeat = UserDTO.PasswordRepeat,
                    Address = UserDTO.Address,
                    PhoneNumber = UserDTO.PhoneNumber,
                    City = UserDTO.City,
                    Country = UserDTO.Country,
                    Zip = UserDTO.Zip,
                    Role = UserDTO.Role
                };

                // Add the new user to the database and save changes
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Return the newly created user with the location of the new resource
                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log the exception and return an internal server error
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion

        #region DeleteUser
        // DELETE: api/Users/{id}
        // Deletes a user by their ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Find the user by their ID
            var user = await _context.Users.FindAsync(id);

            // If the user is not found, return NotFound
            if (user == null)
            {
                return NotFound();
            }

            // Remove the user and save changes to the database
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); // Return NoContent on successful deletion
        }
        #endregion

        #region UserExists
        // Helper method to check if a user exists by their ID
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        #endregion
    }
}
