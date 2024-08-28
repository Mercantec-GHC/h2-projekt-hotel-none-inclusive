
namespace API.Models
{
    public class Login : User
    {
        public required string Password { get; set; }
    }
}
