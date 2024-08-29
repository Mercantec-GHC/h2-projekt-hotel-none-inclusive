using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int? Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        public string LastName { get; set; } = null!;
        [Column("email")]
        public string Email { get; set; } = null!;
        [Column("address")]
        public string? Address { get; set; }
        [Column("phonenumber")]
        public string PhoneNumber { get; set; } = null!;
        [Column("city")]
        public string? City { get; set; }
        [Column("country")]
        public string? Country { get; set; }
        [Column("zip")]
        public string Zip { get; set; } = null!;
        [Column("role")]
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
