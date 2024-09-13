using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using HotelBooking.Data;
using API.Services;

//Forklar meningen med at implementere mapping services i API'en (UserMapping, RoomMapping, BookingMapping)
//Hvordan mapper de i mellem DTO'er og modeller?
// Ville det være et problem at fjerne DTO og mapping services og i stedet returnere modeller direkte fra API'en?

namespace API.Controllers
{
    // Defines an API controller to handle booking operations
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        // Declares fields for database context and mapping services
        private readonly DBContext _context;
        private readonly UserMapping _userMapping;
        private readonly RoomMapping _roomMapping;
        private readonly BookingMapping _bookingMapping;

        // Constructor that injects dependencies for the database context and mapping services
        public BookingController(DBContext context, UserMapping userMapping, RoomMapping roomMapping, BookingMapping bookingMapping)
        {
            _context = context;
            _userMapping = userMapping;
            _roomMapping = roomMapping;
            _bookingMapping = bookingMapping;
        }

        #region GetBookings
        // GET: api/Booking
        // Retrieves all bookings, including user and room data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingWAllData>>> GetBookings()
        {
            // Fetches bookings from the database, including related user and room data
            var bookings = await _context.Bookings.Include(b => b.User).Include(b => b.Room).ToListAsync();

            // Maps the bookings to a model that includes all relevant data
            var bookingWAllData = bookings.Select(b => new BookingWAllData
            {
                BookingDate = b.BookingDate,
                BookingStartDate = b.BookingStartDate,
                BookingEndDate = b.BookingEndDate,
                CheckInTime = b.CheckInTime,
                CheckOutTime = b.CheckOutTime,
                NumberOfNights = b.NumberOfNights,
                UserInfo = _userMapping.MapUserToUserGetDTO(b.User), // Maps the user data to a DTO
                RoomInfo = _roomMapping.MapRoomToGetRoomDTO(b.Room)
            }).ToList();

            // Returns the full booking data
            return Ok(bookingWAllData);
        }
        #endregion

        #region GetBookingWithId
        // GET: api/Booking/5
        // Retrieves a specific booking by its id
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            // Finds the booking based on the provided id
            var bookingDTO = await _context.Bookings.FindAsync(id);

            // If the booking doesn't exist, return 404
            if (bookingDTO == null)
            {
                return NotFound();
            }

            // Returns the booking data
            return Ok(bookingDTO);
        }
        #endregion

        #region PostBooking
        // POST: api/Booking
        // Creates a new booking
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(CreateBookingDTO createBookingDTO)
        {
            // Check if UserId references a valid user
            var user = await _context.Users.FindAsync(createBookingDTO.UserId);
            if (user == null)
            {
                return BadRequest($"User with ID {createBookingDTO.UserId} does not exist.");
            }

            // Adds the new booking to the database after mapping the DTO to the Booking entity
            _context.Bookings.Add(_bookingMapping.MapCreateBookingDTOToBooking(createBookingDTO));
            await _context.SaveChangesAsync();

            // Returns a response indicating that the booking was created successfully
            return CreatedAtAction(nameof(GetBooking), new { id = createBookingDTO.Id }, createBookingDTO);
        }
        #endregion

        #region EditBooking
        // PUT: api/Booking/5
        // Updates an existing booking
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBooking(int id, BookingDTO bookingDTO)
        {
            // If the id in the route does not match the id in the bookingDTO, return a bad request
            if (id != bookingDTO.Id)
            {
                return BadRequest();
            }

            // Mark the bookingDTO entity as modified
            _context.Entry(bookingDTO).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If the booking doesn't exist, return 404
                if (!BookingDTOExists(id))
                {
                    return NotFound();
                }
            }

            // Return 204 No Content on successful update
            return NoContent();
        }
        #endregion

        #region DeleteBooking
        // DELETE: api/Booking/5
        // Deletes a booking by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            // Find the booking by id
            var bookingDTO = await _context.Bookings.FindAsync(id);

            // If the booking doesn't exist, return 404
            if (bookingDTO == null)
            {
                return NotFound();
            }

            // Remove the booking from the database
            _context.Bookings.Remove(bookingDTO);
            await _context.SaveChangesAsync();

            // Return 204 No Content on successful deletion
            return NoContent();
        }
        #endregion

        #region BookingDTOExists
        // Checks if a booking exists by id
        private bool BookingDTOExists(int id)
        {
            // Returns true if a booking with the given id exists in the database
            return _context.Bookings.Any(e => e.Id == id);
        }
        #endregion
    }
}
