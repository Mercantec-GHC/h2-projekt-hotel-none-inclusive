using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordRepeat { get; set; } = null!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Zip { get; set; } = null!;
        public string? Role { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public class UserPostAndPutDTO
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordRepeat { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Zip { get; set; } = null!;
        public string? Role { get; set; }
    }
    
    public class UserGetDTO
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
    }

}
