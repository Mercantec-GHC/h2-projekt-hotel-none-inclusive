using API.Models;
using BootstrapBlazor.Components;

namespace API.Services
{
    public class BookingMapping
    {
        public Booking MapCreateBookingDTOToBooking(CreateBookingDTO booking) // This method maps a CreateBookingDTO object to a Booking object.
        {
            return new Booking
            {
                Id = booking.Id,
                BookingDate = DateTime.UtcNow,
                BookingStartDate = DateTime.SpecifyKind(booking.BookingStartDate.Date,DateTimeKind.Utc),
                BookingEndDate = DateTime.SpecifyKind(booking.BookingEndDate.Date, DateTimeKind.Utc),
                RoomId = booking.RoomId,
                UserId = booking.UserId,
                PaymentStatus = false,
                TotalPrice = booking.TotalPrice
            };
        }
    }
}
