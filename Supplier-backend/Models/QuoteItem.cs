namespace Supplier_backend.Models
{
    public class QuoteItem
    {
        public int QuoteItemId { get; set; }
        public int QuoteId { get; set; }
        public int ProductId { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal QuotedPrice { get; set; }
        public decimal? Discount { get; set; }

        // Navigation properties
        public Quote Quote { get; set; }
        public Product Product { get; set; }
    }
}
