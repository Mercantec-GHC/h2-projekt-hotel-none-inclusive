using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Models;

namespace HotelBooking.Data
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }
    }
}
