namespace TechFix_backend.Models
{
    public class SupplierQuoteItem
    {
        public int SupplierQuoteItemId { get; set; }
        public int SupplierQuoteId { get; set; } // Foreign Key to SupplierQuote
        public int ProductId { get; set; } // Foreign Key to Products
        public decimal QuotedPrice { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal? Discount { get; set; } // Nullable for optional discount

        // Navigation Properties
        public SupplierQuote SupplierQuote { get; set; }
        public ProductRef Product { get; set; }
    }
}
