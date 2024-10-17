namespace TechFix_backend.Models
{
    public class RFQItem
    {
        public int RFQItemId { get; set; }
        public int RFQId { get; set; } // Foreign Key to RFQ
        public int ProductId { get; set; } // Foreign Key to Products
        public int RequestedQuantity { get; set; }
        public int? SupplierQuoteId { get; set; } // Nullable Foreign Key to SupplierQuote

        // Navigation Properties
        public RFQ RFQ { get; set; }
        public ProductRef Product { get; set; } // Product reference from TechFix side
        public SupplierQuote SupplierQuote { get; set; } // Reference to SupplierQuote
    }
}
