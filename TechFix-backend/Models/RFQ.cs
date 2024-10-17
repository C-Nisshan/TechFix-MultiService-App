using TechFix_backend.Enums;

namespace TechFix_backend.Models
{
    public class RFQ
    {
        public int RFQId { get; set; }
        public string CreatedBy { get; set; } // Foreign Key to TechFixUsers
        public DateTime CreationDate { get; set; }
        public RFQStatus Status { get; set; } // Enum for status
        public int? SupplierId { get; set; } // Optional, Foreign Key to SupplierDb.Suppliers

        // Navigation Properties
        public ICollection<RFQItem> RFQItems { get; set; }
        public ICollection<SupplierQuote> SupplierQuotes { get; set; }
        public Supplier Supplier { get; set; } // Reference to Supplier
    }
}
