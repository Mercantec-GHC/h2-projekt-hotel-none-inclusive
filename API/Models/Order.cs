using API.Models;

namespace HotelBooking.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderPlace {  get; set; }
        public DateTime? OrderFulfilled { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}