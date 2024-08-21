using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Models;

namespace HotelBooking.Data
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }
    }
}
