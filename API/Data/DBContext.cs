using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HotelBooking.Data
{
    public class DBContext : DbContext
    {
        public DbSet<Customer> customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=ep-bitter-salad-a2i17tas.eu-central-1.aws.neon.tech;Port=5432;Database=H2HotelBooking;Username=HotelNonInclusive;Password=N3rtzsXQjDF9");
        }
    }
}
