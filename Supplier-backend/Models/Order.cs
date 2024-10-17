namespace Supplier_backend.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public Client Client { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
