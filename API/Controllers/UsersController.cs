using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.Select(user => new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }).ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserEditDTO userEditDTO)
        {
            // Find den eksisterende bruger i databasen
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Opdater kun de felter, der er inkluderet i UserDTO
            user.FirstName = userEditDTO.FirstName ?? throw new ArgumentNullException(nameof(userEditDTO.FirstName));
            user.LastName = userEditDTO.LastName ?? throw new ArgumentNullException(nameof(userEditDTO.LastName));
            user.Email = userEditDTO.Email ?? throw new ArgumentNullException(nameof(userEditDTO.Email));
            user.Address = userEditDTO.Address ?? throw new ArgumentNullException(nameof(userEditDTO.Address));
            user.PhoneNumber = userEditDTO.PhoneNumber ?? throw new ArgumentNullException(nameof(userEditDTO.PhoneNumber));
            user.City = userEditDTO.City ?? throw new ArgumentNullException(nameof(userEditDTO.City));
            user.Country = userEditDTO.Country ?? throw new ArgumentNullException(nameof(userEditDTO.Country));
            user.Zip = userEditDTO.Zip ?? throw new ArgumentNullException(nameof(userEditDTO.Zip));


            // Marker den opdaterede bruger som ændret
            _context.Entry(user).State = EntityState.Modified;

            // Forsøg at gemme ændringerne
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
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
        public async Task<ActionResult<User>> PostUser(UserCreateDTO userCreateDTO)
        {
            User user = new User()
            {
                FirstName = userCreateDTO.FirstName,
                LastName = userCreateDTO.LastName,
                Email = userCreateDTO.Email,
                Address = userCreateDTO.Address,
                PhoneNumber = userCreateDTO.PhoneNumber,
                City = userCreateDTO.City,
                Country = userCreateDTO.Country,
                Zip = userCreateDTO.Zip,
                Role = "Customer"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
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
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
