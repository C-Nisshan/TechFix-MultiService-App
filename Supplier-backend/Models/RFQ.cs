using Supplier_backend.Enums;
using System.Text.Json.Serialization;

namespace Supplier_backend.Models
{
    public class RFQ
    {
        public int RFQId { get; set; }
        public DateTime CreationDate { get; set; }
        public RFQStatus Status { get; set; }

        [JsonIgnore] // This hides the numeric value in the JSON response
        public string StatusString => Status.ToString();

        // Navigation properties
        public ICollection<RFQItem> RFQItems { get; set; }
        public ICollection<Quote>? Quotes { get; set; }
    }
}
