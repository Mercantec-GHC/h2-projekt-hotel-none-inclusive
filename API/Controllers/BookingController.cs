using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingWAllData>>> GetBookings()
        {
            var bookings = await _context.Bookings.Include(b => b.User).Include(b => b.Room).ToListAsync();

            var bookingWAllData = bookings.Select(b => new BookingWAllData
            {
                BookingDate = b.BookingDate,
                BookingStartDate = b.BookingStartDate,
                BookingEndDate = b.BookingEndDate,
                CheckInTime = b.CheckInTime,
                CheckOutTime = b.CheckOutTime,
                NumberOfNights = b.NumberOfNights,
                UserInfo = _userMapping.MapUserToUserGetDTO(b.User),
                RoomInfo = _roomMapping.MapRoomToGetRoomDTO(b.Room)
            }).ToList();

            return Ok(bookingWAllData);
        }

        // GET: api/Booking/5
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
       

        // POST: api/Booking
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(CreateBookingDTO createBookingDTO)
        {
            // Tjek om UserId refererer til en gyldig bruger
            var user = await _context.Users.FindAsync(createBookingDTO.UserId);
            if (user == null)
            {
                return BadRequest($"User with ID {createBookingDTO.UserId} does not exist.");
            }
            _context.Bookings.Add(_bookingMapping.MapCreateBookingDTOToBooking(createBookingDTO));
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = createBookingDTO.Id }, createBookingDTO);
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBooking(int id, BookingDTO bookingDTO)
        {
            if (id != bookingDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookingDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingDTOExists(id))
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

        // DELETE: api/Booking/5
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

        private bool BookingDTOExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        

        
    }
}
