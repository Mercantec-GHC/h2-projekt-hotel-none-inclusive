using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("bookings")]
    public class Booking
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("booking_date")]
        public DateTime BookingDate { get; set; }
        [Column("booking_start_date")]
        public DateTime BookingStartDate { get; set; }
        [Column("booking_end_date")]
        public DateTime BookingEndDate { get; set; }

        [Column("rooms")]
        public List<Room> Rooms { get; set; } = new List<Room>();

        [Column("check_in_time")]
        public DateTime CheckInTime { get; set; }
        [Column("check_out_time")]
        public DateTime CheckOutTime { get; set; }
        [Column("number_of_nights")]
        public int NumberOfNights { get; set; }
        [Column("price_per_night")]
        public int PricePerNight { get; set; }
        [Column("reservation_id")]
        public int ReservationID { get; set; }
        
        // PaymentStatus (String = Unpaid/Paid)
        [Column("payment_status")]
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
