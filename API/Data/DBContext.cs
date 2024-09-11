using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Models;

namespace HotelBooking.Data
{
    // This class defines the database context for the application, inheriting from DbContext.
    public class DBContext : DbContext
    {
        // DbSet to represent the 'Users' table in the database.
        public DbSet<User> Users { get; set; }

        // DbSet to represent the 'Bookings' table in the database.
        public DbSet<Booking> Bookings { get; set; }

        // DbSet to represent the 'Rooms' table in the database.
        public DbSet<Room> Rooms { get; set; }

        // Constructor that accepts options for configuring the DbContext.
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }
    }
}
