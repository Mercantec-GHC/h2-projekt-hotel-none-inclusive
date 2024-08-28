namespace API.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Zip { get; set; } = null!;
        public string? Role { get; set; }
    }

    public class UserDTO
    {
        public int? Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Zip { get; set; } = null!;
        public string? Role { get; set; }
    }

}
