using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using HotelBooking.Data;
using API.Services;

namespace API.Controllers
{
    // Marks the controller as an API controller and defines the route for requests
    [ApiController]
    [Route("api/[Controller]")]
    public class RoomsController : Controller
    {
        // Fields for the database context and room mapping service
        private readonly DBContext _context;
        private readonly RoomMapping _roomMapping;

        // Constructor to inject the database context and mapping service
        public RoomsController(DBContext context, RoomMapping roomMapping)
        {
            _context = context;
            _roomMapping = roomMapping;
        }

    
        // GET: api/Rooms
        // Retrieves a list of rooms with selected fields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetRoomDTO>>> GetRooms()
        {
            // Retrieves all rooms from the database and maps them to GetRoomDTO
            var rooms = await _context.Rooms.Select(room => new GetRoomDTO // Selects specific fields from the room entity
            {
                Id = room.Id,
                Price = room.PricePerNight,
                RoomType = room.RoomType,
                Description = room.Description,
                ImageURL = room.ImageURL,
                RoomNumber = room.RoomNumber,
                IsOccupied = room.IsOccupied,
                Floor = room.Floor
            }).ToListAsync();

            // Returns the list of rooms
            return Ok(rooms);
        }
     

       
        // GET: api/Rooms/Details/5
        // Retrieves the details of a specific room based on its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GetRoomDTO>> Details(int id)
        {
            // Checks if the provided ID is null
            if (id == null)
            {
                return NotFound();
            }

            // Finds the room in the database by its ID
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Maps the room entity to GetRoomDTO and returns it
            return Ok(_roomMapping.MapRoomToGetRoomDTO(room));
        }
      

      
        // PUT: api/Rooms/Edit/5
        // Updates the details of a specific room
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            // Checks if the room ID is null
            if (id == null)
            {
                return NotFound();
            }

            // Finds the room in the database by its ID
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Returns the found room for editing
            return Ok(room);
        }
        

     
        // POST: api/Rooms
        // Creates a new room entry in the database
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(CreateRoomDTO createRoomDTO)
        {
            // Checks if the provided room data is null
            if (createRoomDTO == null)
            {
                return BadRequest("Room data is null.");
            }

            // Validates the input data against the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // Check if the room number is already in use
            
            if (await _context.Rooms.AnyAsync(r => r.RoomNumber == createRoomDTO.RoomNumber))
            {
                return BadRequest("Room number is already in use.");
            }

            // Maps CreateRoomDTO to the Room entity
            var room = new Room
            {
                RoomType = createRoomDTO.RoomType,
                PricePerNight = createRoomDTO.PricePerNight,
                Description = createRoomDTO.Description,
                ImageURL = createRoomDTO.ImageURL,
                RoomNumber = createRoomDTO.RoomNumber,
                IsOccupied = false,
                Floor = createRoomDTO.Floor
            };

            // Adds the new room to the database and saves changes
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Returns a response with the newly created room's details
            return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
        }
        

        
        // DELETE: api/Rooms/Delete/5
        // Deletes a specific room from the database
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            // Checks if the room ID is null
            if (id == null)
            {
                return NotFound();
            }

            // Finds the room in the database by its ID
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            // Returns the room that is going to be deleted
            return Ok(room);
        }
      
        //Check Room Availability, count days and calculate total price (EGEN NOTE: BRUG I POST BOOKING)
        [HttpGet("CheckRoomAvailability")]
        public async Task<ActionResult<string>> CheckRoomAvailability(int roomId, DateTime startDate, DateTime endDate)
        {
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc); 
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            bool isAvailable = !await _context.Bookings
                .AnyAsync(b => b.RoomId == roomId &&
                               ((b.BookingStartDate <= startDate && b.BookingEndDate >= startDate) ||
                                (b.BookingStartDate <= endDate && b.BookingEndDate >= endDate) ||
                                (b.BookingStartDate >= startDate && b.BookingEndDate <= endDate)));

            if (!isAvailable)
            {
                return NotFound("The room is not available");
            }

            // Retrieve the room details to get the price per night
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            decimal totalPrice = 0;
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                decimal pricePerNight = room.PricePerNight;
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

            return Ok($"The room is available. Total price is {totalPrice}");
        }
    }
}
