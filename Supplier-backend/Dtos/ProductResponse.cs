// ProductResponseDto.cs
namespace Supplier_backend.Dtos
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockLevel { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
