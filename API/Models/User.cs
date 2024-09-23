using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    // Represents a User entity in the system
    public class User
    {
        // Unique identifier for the user
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Zip { get; set; } = null!;
        
        
        public List<Booking> Bookings { get; set; } = new List<Booking>(); // Navigation property
    }

    // DTO for creating or updating a user (used in POST and PUT operations)
    public class UserPostAndPutDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Zip { get; set; } = null!;
        
    }

    // DTO for retrieving user details (used in GET operations)
    public class UserGetDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Zip { get; set; } = null!;
        
    }

    // DTO for user login (used during authentication)
    public class UserLoginDTO 
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
       
    }
}
