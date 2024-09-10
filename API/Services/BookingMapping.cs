using API.Models;
using BootstrapBlazor.Components;

namespace API.Services
{
    public class BookingMapping
    {
        public Booking MapCreateBookingDTOToBooking(CreateBookingDTO booking)
        {
            return new Booking
            {
                Id = booking.Id,
                BookingDate = DateTime.UtcNow,
                BookingStartDate = DateTime.SpecifyKind(booking.BookingStartDate,DateTimeKind.Utc),
                BookingEndDate = DateTime.SpecifyKind(booking.BookingEndDate, DateTimeKind.Utc),
                RoomId = booking.RoomId,
                UserId = booking.UserId,
                PaymentStatus = "",
                CheckInTime = DateTime.UtcNow,
                CheckOutTime = DateTime.UtcNow,
                NumberOfNights = (booking.BookingEndDate - booking.BookingStartDate).Days
            };
        }
    }
}
