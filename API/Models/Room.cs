using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("rooms")]
    public class Room
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("room_number")]
        public int RoomNumber { get; set; }
        [Column("room_type")]
        public string RoomType { get; set; } = null!;
        [Column("price_per_night")]
        public int PricePerNight { get; set; }
        [Column("is_occupied")]
        public bool IsOccupied { get; set; }
        [Column("floor")]
        public int Floor { get; set; }

    }
}
