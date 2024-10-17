namespace TechFix_backend.Models
{
    public class ProductRef
    {   
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }

        // Navigation Properties
        public Supplier Supplier { get; set; }
        public ICollection<SupplierQuoteItem> QuoteItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
