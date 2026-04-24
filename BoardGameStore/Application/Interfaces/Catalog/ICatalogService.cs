using Application.DTO.Input;
using Application.DTO.Output;
using Application.DTO.Output.FilterDTO;
using Application.DTO.Output.ProductDTO;

namespace Application.Interfaces
{
    public interface ICatalogService
    {
        public Task<PageResultDto<ProductListDto>> GetProductsAsync(ProductQuery pq, CancellationToken ct);
        public Task<ProductDetailsDto?> GetProductAsync(string idOrSlug, CancellationToken ct);
        public Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken ct);
    }
}
