namespace API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public string HotelName { get; set; } = null!;
        public int RoomNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public int NumberOfNights { get; set; }
        public int PricePerNight { get; set; }
        public int ReservationID { get; set; }
        
        // Match UserName, UserEmail, UserPhoneNumber from User table to use it
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;
    }

    public class BookingDTO
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public string HotelName { get; set; } = null!;
        public int RoomNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public int NumberOfNights { get; set; }
        public int PricePerNight { get; set; }
        public int ReservationID { get; set; }

        // Match UserName, UserEmail, UserPhoneNumber from User table to use it
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        // PaymentStatus (String = Unpaid/Paid)
        public string PaymentStatus { get; set; } = null!;
    }

}
