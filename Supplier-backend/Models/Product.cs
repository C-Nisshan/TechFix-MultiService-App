namespace Supplier_backend.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockLevel { get; set; }

        public byte[]? ImageData { get; set; }

        // Navigation properties
        public ICollection<QuoteItem> QuoteItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public ICollection<RFQItem> RFQItems { get; set; }
        public Inventory Inventory { get; set; }

    }
}
