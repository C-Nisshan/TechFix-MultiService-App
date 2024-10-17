namespace Supplier_backend.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string ContactInfo { get; set; }

        // Navigation properties
        public ICollection<Quote> Quotes { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
