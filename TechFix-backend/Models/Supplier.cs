namespace TechFix_backend.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; set; }
        public ICollection<ProductRef> ProductRefs { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
        public ICollection<SupplierQuote> SupplierQuotes { get; set; } 
    }
}
