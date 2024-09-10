using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }

        public Room Room { get; set; } = null!;

        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public int NumberOfNights { get; set; }
        
        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;
        public int RoomId { get; set; }
        // Foreign key to User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

    }

    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public int NumberOfNights { get; set; }
        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;
        public int RoomId { get; set; }
        public int UserId { get; set; }

    }

    public class CreateBookingDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public int NumberOfNights { get; set; }

        public int RoomId { get; set; }
        public int UserId { get; set; }
    }


    public class BookingWAllData
    {
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public int NumberOfNights { get; set; }


        public UserGetDTO UserInfo { get; set; }
        public GetRoomDTO RoomInfo { get; set; }

    }

}
