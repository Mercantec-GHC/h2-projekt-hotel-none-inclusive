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
        public int PricePerNight { get; set; }
        public int ReservationID { get; set; }
        
        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;

    }

    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public int RoomNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public int NumberOfNights { get; set; }
        public int PricePerNight { get; set; }
        public int ReservationID { get; set; }


        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;
    }

}
