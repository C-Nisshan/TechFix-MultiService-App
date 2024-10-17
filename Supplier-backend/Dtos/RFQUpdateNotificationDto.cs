using Supplier_backend.Enums;

namespace Supplier_backend.Dtos
{
    public class RFQUpdateNotificationDto
    {
        public RFQStatus? Status { get; set; } // Nullable if you want to allow partial updates
    }
}
