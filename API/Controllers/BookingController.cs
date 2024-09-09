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
                PricePerNight = b.PricePerNight,
                UserInfo = MapUserToUserGetDTO(b.User),
                RoomInfo = MapRoomToGetRoomDTO(b.Room)
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
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDTO)
        {
            // Tjek om UserId refererer til en gyldig bruger
            var user = await _context.Users.FindAsync(bookingDTO.UserId);
            if (user == null)
            {
                return BadRequest($"User with ID {bookingDTO.UserId} does not exist.");
            }
            Booking booking = new Booking()
            {
                BookingDate = bookingDTO.BookingDate,
                BookingStartDate = bookingDTO.BookingStartDate,
                BookingEndDate = bookingDTO.BookingEndDate,
                CheckInTime = bookingDTO.CheckInTime,
                CheckOutTime = bookingDTO.CheckOutTime,
                NumberOfNights = bookingDTO.NumberOfNights,
                PricePerNight = bookingDTO.PricePerNight,
                ReservationID = bookingDTO.ReservationID,
                PaymentStatus = bookingDTO.PaymentStatus,
                RoomId = bookingDTO.RoomId,
                UserId = bookingDTO.UserId
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

        private UserGetDTO MapUserToUserGetDTO(User user)
        {
            UserGetDTO userGetDTO = new UserGetDTO()
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName
            };


            return userGetDTO;
        }

        private GetRoomDTO MapRoomToGetRoomDTO(Room room)
        {
            GetRoomDTO getRoomDTO = new GetRoomDTO()
            {
                Id = room.Id,
                Price = room.PricePerNight,
                RoomType = room.RoomType,
                Description = room.Description,
                ImageURL = room.ImageURL
            };

            return getRoomDTO;
        }
    }
}
