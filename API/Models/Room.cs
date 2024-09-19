using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    // Represents a Room entity in the system
    public class Room
    {
        // Unique identifier for the room
        public int Id { get; set; }
        
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public int PricePerNight { get; set; }
        public int Floor { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }


    }

    // DTO for retrieving room information
    public class GetRoomDTO
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string RoomType { get; set; } = null!;
        public string Description { get; set; }
        public string ImageURL { get; set; }
        
        public int RoomNumber { get; set; } 
        public int Floor { get; set; }

    }

    // DTO for creating a new room
    public class CreateRoomDTO
    {
        public int Id { get; set; }
        
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public int PricePerNight { get; set; }
        public int Floor { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
    
    public class UpdateRoomDTO
    {
        public string RoomType { get; set; }
        public int PricePerNight { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int RoomNumber { get; set; }
        public int Floor { get; set; }
    }
}
