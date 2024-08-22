using HotelBooking.Data;
using HotelBooking.Models;

namespace API.Models
{
    public class SignUp : User
    {
        private readonly DBContext _dbContext;

        public SignUp(DBContext dbContext)
        {
            _dbContext = dbContext;
        }


        // Method to register a user with dynamic parameters
        public void SignUpRegistreUser(string firstName, string lastName, string email, string address, string city, string country, string zip)
        {
            try
            {
                // Set user properties
                this.FirstName = firstName;
                this.LastName = lastName;
                this.Email = email;
                this.Address = address;
                this.City = city;
                this.Country = country;
                this.Zip = zip;


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