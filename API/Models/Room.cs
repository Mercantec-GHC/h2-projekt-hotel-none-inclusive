using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public int PricePerNight { get; set; }
        public bool IsOccupied { get; set; }
        public int Floor { get; set; }

    }
}
