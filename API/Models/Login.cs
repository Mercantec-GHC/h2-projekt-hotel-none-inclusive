using HotelBooking.Models;

namespace API.Models
{
    public class Login : Customer
    {
        public required string Password { get; set; }
    }
}
