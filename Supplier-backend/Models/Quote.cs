namespace Supplier_backend.Models
{
    public class Quote
    {
        public int QuoteId { get; set; }
        public int RFQId { get; set; }
        public int ClientId { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryTime { get; set; }
        public DateTime ResponseDate { get; set; } 

        // Navigation properties
        public RFQ RFQ { get; set; }
        public Client Client { get; set; }
        public ICollection<QuoteItem> QuoteItems { get; set; }
    }
}
