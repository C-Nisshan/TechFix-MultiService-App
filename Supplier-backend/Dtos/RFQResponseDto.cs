using Supplier_backend.Enums;

namespace Supplier_backend.Dtos
{
    public class RFQResponseDto
    {
        public int RFQId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public List<RFQItemResponseDto> RFQItems { get; set; }
    }
}
