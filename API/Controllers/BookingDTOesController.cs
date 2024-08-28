using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using HotelBooking.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DBContext _context;

        public BookingController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
        {
            return Ok(await _context.Bookings.ToListAsync());
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
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDTO)
        {
            Booking booking = new Booking()
            {
                BookingDate = bookingDTO.BookingDate,
                BookingStartDate = bookingDTO.BookingStartDate,
                BookingEndDate = bookingDTO.BookingEndDate,
                HotelName = bookingDTO.HotelName,
                RoomNumber = bookingDTO.RoomNumber,
                CheckInTime = bookingDTO.CheckInTime,
                CheckOutTime = bookingDTO.CheckOutTime,
                NumberOfNights = bookingDTO.NumberOfNights,
                PricePerNight = bookingDTO.PricePerNight,
                ReservationID = bookingDTO.ReservationID,
                PaymentStatus = bookingDTO.PaymentStatus
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = bookingDTO.Id }, bookingDTO);
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
