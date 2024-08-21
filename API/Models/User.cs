namespace API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public required string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string Zip { get; set; }
        public string? Role { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
