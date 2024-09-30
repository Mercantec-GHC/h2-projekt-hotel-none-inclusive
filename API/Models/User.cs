using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    // Represents a User entity in the system
    public class User
    {
        // Unique identifier for the user
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        
        public bool IsAdmin { get; set; } = false;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        
        
        public List<Booking> Bookings { get; set; } = new List<Booking>(); // Navigation property
    }

    // DTO for creating or updating a user (used in POST and PUT operations)
    public class UserPostAndPutDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        
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
        public string? Zip { get; set; }
        public bool? IsAdmin { get; set; }

}

    // DTO for user login (used during authentication)
    public class UserLoginDTO 
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
       
    }
}
