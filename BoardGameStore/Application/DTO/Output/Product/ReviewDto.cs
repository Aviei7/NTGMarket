using Domain.Models.ProductsModel;

namespace Application.DTO.Output.ProductDTO
{
    public record class ReviewDto
    {
        public ProductModel Product { get; init; }
        public byte Rating { get; init; }
        public string? Text { get; init; } = null;

    };
}