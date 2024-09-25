using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using HotelBooking.Data;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly UserMapping _userMapping;
        private readonly RoomMapping _roomMapping;
        private readonly BookingMapping _bookingMapping;

        public BookingController(DBContext context, UserMapping userMapping, RoomMapping roomMapping, BookingMapping bookingMapping)
        {
            _context = context;
            _userMapping = userMapping;
            _roomMapping = roomMapping;
            _bookingMapping = bookingMapping;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingWAllData>>> GetBookings()
        {
            var bookings = await _context.Bookings.Include(b => b.User).Include(b => b.Room).ToListAsync();
            var bookingWAllData = bookings.Select(b => new BookingWAllData
            {
                BookingDate = b.BookingDate,
                BookingStartDate = b.BookingStartDate,
                BookingEndDate = b.BookingEndDate,
                UserInfo = _userMapping.MapUserToUserGetDTO(b.User),
                RoomInfo = _roomMapping.MapRoomToGetRoomDTO(b.Room)
            }).ToList();

            return Ok(bookingWAllData);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            var bookingDTO = await _context.Bookings.FindAsync(id);
            if (bookingDTO == null)
            {
                return NotFound();
            }
            return Ok(bookingDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(CreateBookingDTO createBookingDTO)
        {
            var user = await _context.Users.FindAsync(createBookingDTO.UserId);
            if (user == null)
            {
                return BadRequest($"User with ID {createBookingDTO.UserId} does not exist.");
            }

            if (createBookingDTO.BookingStartDate > createBookingDTO.BookingEndDate)
            {
                return BadRequest("The booking start date must be before the end date.");
            }

            var isRoomBooked = await _context.Bookings
                .AnyAsync(b => b.RoomId == createBookingDTO.RoomId &&
                               b.BookingStartDate < createBookingDTO.BookingEndDate &&
                               b.BookingEndDate >= createBookingDTO.BookingStartDate);

            if (isRoomBooked)
            {
                return BadRequest("The room is already booked for the specified date range.");
            }

            var room = await _context.Rooms.FindAsync(createBookingDTO.RoomId);
            if (room == null)
            {
                return BadRequest("Room not found.");
            }

            decimal totalPrice = 0;
            for (DateTime date = createBookingDTO.BookingStartDate; date < createBookingDTO.BookingEndDate; date = date.AddDays(1))
            {
                decimal pricePerNight = room.PricePerNight;
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    pricePerNight *= 1.20m;
                }
                if (date.Month == 7)
                {
                    pricePerNight *= 1.15m;
                }

                totalPrice += pricePerNight;
            }

            var booking = _bookingMapping.MapCreateBookingDTOToBooking(createBookingDTO);
            booking.TotalPrice = totalPrice;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = createBookingDTO.Id }, createBookingDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditBooking(int id, BookingDTO bookingDTO)
        {
            if (id != bookingDTO.Id)
            {
                return BadRequest();
            }

            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var bookingDTO = await _context.Bookings.FindAsync(id);
            if (bookingDTO == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(bookingDTO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        // New endpoint to get bookings by user's email
        [HttpGet("user/{email}")]
        public async Task<ActionResult<IEnumerable<BookingWAllData>>> GetBookingsByUserEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found.");
            }

            var bookings = await _context.Bookings
                .Where(b => b.UserId == user.UserId)
                .Include(b => b.User)
                .Include(b => b.Room)
                .ToListAsync();

            var bookingWAllData = bookings.Select(b => new BookingWAllData
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                BookingStartDate = b.BookingStartDate,
                BookingEndDate = b.BookingEndDate,
                UserInfo = _userMapping.MapUserToUserGetDTO(b.User),
                RoomInfo = _roomMapping.MapRoomToGetRoomDTO(b.Room)
            }).ToList();

            return Ok(bookingWAllData);
        }
    }
}