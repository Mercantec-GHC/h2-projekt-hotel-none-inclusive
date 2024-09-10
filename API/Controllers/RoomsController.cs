using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Models;
using HotelBooking.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RoomsController : Controller
    {
        private readonly DBContext _context;

        public RoomsController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetRoomDTO>>> GetRooms()
        {
            var rooms = await _context.Rooms.Select(room => new GetRoomDTO
            {
                Id = room.Id,
                Price = room.PricePerNight,
                RoomType = room.RoomType,
                Description = room.Description,
                ImageURL = room.ImageURL
            }).ToListAsync();

            return Ok(rooms);
        }



        // GET: Rooms/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetRoomDTO>> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // GET: Rooms/Edit/5
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }
        // POST: api/Rooms
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(CreateRoomDTO createRoomDTO)
        {
            if (createRoomDTO == null)
            {
                return BadRequest("Room data is null.");
            }

            // Validate the input data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map CreateRoomDTO to Room entity
            var room = new Room
            {
                RoomType = createRoomDTO.RoomType,
                PricePerNight = createRoomDTO.PricePerNight,
                Description = createRoomDTO.Description,
                ImageURL = createRoomDTO.ImageURL
            };

            // Add the new room to the database
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Return a response with the newly created room
            return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
        }


        // GET: Rooms/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        //private bool RoomExists(int id)
        //{
        //    return _context.Rooms.Any(e => e.Id == id);
        //}
    }
}
