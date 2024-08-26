namespace API.Models
{
    public class User
    {
        public int? Id { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public string? Zip { get; set; }
        public string? Role { get; set; }
    }

    public class UserDTO
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
    }

    public class UserEditDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        public string? Role { get; set; }
    }


    public class UserCreateDTO
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
    }
}
