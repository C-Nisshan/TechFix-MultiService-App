namespace Supplier_backend.Models
{
    public class RFQItem
    {
        public int RFQItemId { get; set; }
        public int RFQId { get; set; }
        public int ProductId { get; set; }
        public int RequestedQuantity { get; set; }

        // Navigation properties
        public RFQ RFQ { get; set; }
        public Product Product { get; set; }
    }
}
