namespace Application.DTO.Output.ProductDTO
{
    public record class ProductListDto {
        public int Id { get; init; }
        public string? Name { get; init; } = null;
        public string? Slug { get; init; } = null;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public int CategoryId { get; init; }
        public string? CategoryName { get; init; } = null;
        public string? PrimaryImageUrl { get; init; } = null;
        public double? AvgRating { get; init; }
        public string? Description { get; init ; }

    };
}
