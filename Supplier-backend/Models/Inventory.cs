namespace Supplier_backend.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int StockLevel { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation property
        public Product Product { get; set; }
    }
}
