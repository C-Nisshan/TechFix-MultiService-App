namespace TechFix_backend.Models
{
    public class SupplierQuote
    {
        public int SupplierQuoteId { get; set; }
        public int RFQId { get; set; } // Foreign Key to RFQ
        public int SupplierId { get; set; } // Foreign Key to SupplierDb.Suppliers
        public decimal TotalPrice { get; set; }
        public string DeliveryTime { get; set; } // e.g., "5 days", "2 weeks"
        public DateTime QuoteReceivedDate { get; set; }
        public bool SelectedQuote { get; set; } // Indicates if this quote was selected by TechFix

        // Navigation Properties
        public RFQ RFQ { get; set; }
        public Supplier Supplier { get; set; } // Reference to Supplier
        public ICollection<SupplierQuoteItem> SupplierQuoteItems { get; set; }
    }
}
