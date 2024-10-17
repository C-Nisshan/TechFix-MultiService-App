using Supplier_backend.Enums;

namespace Supplier_backend.Dtos
{
    public class RFQNotificationDto
    {
        public int RFQId { get; set; }

        public DateTime CreationDate { get; set; }
        public RFQStatus Status { get; set; }
        public List<RFQItemNotificationDto> RFQItems { get; set; }
    }

}
