namespace TechFix_backend.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int SupplierId { get; set; }
        public int QuoteId { get; set; }
        public int PlacedBy { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        // Navigation Properties
        public Supplier Supplier { get; set; }
        public User User { get; set; }
        public SupplierQuote Quote { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
