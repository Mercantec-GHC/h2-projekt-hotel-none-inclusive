namespace HotelBooking.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;
    }
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

    }
}
