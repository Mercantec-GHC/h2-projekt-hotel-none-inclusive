using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using HotelBooking.Data;
using API.Services;

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
            
            //Check if the booking start date is before the end date
            if (createBookingDTO.BookingStartDate > createBookingDTO.BookingEndDate)
            {
                return BadRequest("The booking start date must be before the end date.");
            }
            
            // Fetch the first available room of the requested room type
            var availableRoom = await _context.Rooms
                .Where(r => r.RoomType == createBookingDTO.RoomType) 
                .Where(r => !_context.Bookings
                    .Any(b => b.RoomId == r.Id &&
                              b.BookingStartDate < createBookingDTO.BookingEndDate &&
                              b.BookingEndDate >= createBookingDTO.BookingStartDate)) // Room availability check
                .FirstOrDefaultAsync();

            if (availableRoom == null)
            {
                return BadRequest("No available rooms of the requested type for the specified date range.");
            }
            
        

            // Calculate the total price
            decimal totalPrice = 0;
            for (DateTime date = createBookingDTO.BookingStartDate; date < createBookingDTO.BookingEndDate; date = date.AddDays(1)) //Iterates through each day in the date range
            {
                decimal pricePerNight =  availableRoom.PricePerNight;
                // Apply 20% increase for weekends
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    pricePerNight *= 1.20m;
                }
                // Apply 15% increase for July
                if (date.Month == 7)
                {
                    pricePerNight *= 1.15m;
                }
        
                totalPrice += pricePerNight;
            }

            // Map the DTO to the Booking entity and set the TotalPrice
            var booking = _bookingMapping.MapCreateBookingDTOToBooking(createBookingDTO);
            booking.TotalPrice = totalPrice;
            booking.RoomId = availableRoom.Id; // Assign the available room to the booking


            // Adds the new booking to the database
            _context.Bookings.Add(booking);
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
            if (id != bookingDTO.Id)
            {
                return BadRequest();
            }

            // Retrieve the existing booking entity
            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            // Update the properties of the existing booking entity
            existingBooking.BookingDate = bookingDTO.BookingDate;
            existingBooking.BookingStartDate = bookingDTO.BookingStartDate;
            existingBooking.BookingEndDate = bookingDTO.BookingEndDate;
            existingBooking.RoomId = bookingDTO.RoomId;
            existingBooking.UserId = bookingDTO.UserId;
            existingBooking.PaymentStatus = bookingDTO.PaymentStatus;
          

            _context.Entry(existingBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
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
