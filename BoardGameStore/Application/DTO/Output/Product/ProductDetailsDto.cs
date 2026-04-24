namespace Application.DTO.Output.ProductDTO
{
    public record class ProductDetailsDto
    {
        public int ID { get; init; }
        public string? Name { get; init; } = null;
        public string? Slug { get; init; } = null;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public int CategoryID { get; init; }
        public string? Description { get; init; } = null;
        public DateTime CreatedAt { get; init; }
        public IReadOnlyList<ProductImageDto>? Images { get; init; }
        public double? AvgRating { get; init; }
        //int ReviewsCount { get; init; }

    };
}
