using HotelBooking.Data;
using HotelBooking.Models;

namespace API.Models
{
    public class SignUp : User
    {
        private readonly DBContext _context;

        // Constructor to inject the DbContext
        public SignUp(DBContext context)
        {
            _context = context;
        }

        public void SignUpRegistreUser()
        {
            try
            {
                // Adding new user - This to use DbSet
                _context.Users.Add(this);

                // Save changes to database
                _context.SaveChanges();

                Console.WriteLine("User registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
