using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    // This class represents a booking entity in the database.
    public class Booking
    {
        // Unique identifier for the booking.
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public Room Room { get; set; } = null!;

        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;
        public int RoomId { get; set; }
        // Foreign key to User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        
        public decimal TotalPrice { get; set; }

    }

    // DTO (Data Transfer Object) for transferring booking information.
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;
        // Foreign key referencing the Room entity.
        public int RoomId { get; set; }
        // Foreign key referencing the User entity.
        public int UserId { get; set; }
        
        public decimal TotalPrice { get; set; }

    }

    public class CreateBookingDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        
        // Foreign key referencing the Room entity.
        public int RoomId { get; set; }
        // Foreign key referencing the User entity.
        public int UserId { get; set; }
        
        public decimal TotalPrice { get; set; }
    }

    // DTO for transferring booking information with additional room and user data.
    public class BookingWAllData
    {
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        // Information about the user who made the booking.
        public UserGetDTO UserInfo { get; set; }
        // Information about the room that was booked.
        public GetRoomDTO RoomInfo { get; set; }
    }

}
