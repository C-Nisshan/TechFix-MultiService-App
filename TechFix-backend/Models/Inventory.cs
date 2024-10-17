namespace TechFix_backend.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int StockLevel { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation Properties
        public Supplier Supplier { get; set; }
        public ProductRef Product { get; set; }
    }
}
