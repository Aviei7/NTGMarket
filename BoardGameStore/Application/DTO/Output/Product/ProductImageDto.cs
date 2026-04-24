namespace Application.DTO.Output.ProductDTO
{
    public record class ProductImageDto { 
    
        public int ID { get; init; }
        public string? Url { get; init; } = null;
        public bool IsPrimary { get; init; }
    };
}
