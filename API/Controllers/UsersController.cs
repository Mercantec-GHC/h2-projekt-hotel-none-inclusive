using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGetDTO>>> GetUsers()
        {
            var users = await _context.Users.Select(user => new UserGetDTO
                {
                    Id = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName

                }).ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserPostAndPutDTO UserDTO)
        {
            // Find den eksisterende bruger i databasen
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Opdater kun de felter, der er inkluderet i UserDTO
            user.FirstName = UserDTO.FirstName ?? throw new ArgumentNullException(nameof(UserDTO.FirstName));
            user.LastName = UserDTO.LastName ?? throw new ArgumentNullException(nameof(UserDTO.LastName));
            user.Email = UserDTO.Email ?? throw new ArgumentNullException(nameof(UserDTO.Email));
            user.Password = UserDTO.Password ?? throw new ArgumentNullException(nameof(UserDTO.Password));
            user.Address = UserDTO.Address ?? throw new ArgumentNullException(nameof(UserDTO.Address));
            user.PhoneNumber = UserDTO.PhoneNumber ?? throw new ArgumentNullException(nameof(UserDTO.PhoneNumber));
            user.City = UserDTO.City ?? throw new ArgumentNullException(nameof(UserDTO.City));
            user.Country = UserDTO.Country ?? throw new ArgumentNullException(nameof(UserDTO.Country));
            user.Zip = UserDTO.Zip ?? throw new ArgumentNullException(nameof(UserDTO.Zip));


            // Marker den opdaterede bruger som ændret
            _context.Entry(user).State = EntityState.Modified;

            // Forsøg at gemme ændringerne
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.UserId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserPostAndPutDTO UserDTO)
        {
            try
            {
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
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }




        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
