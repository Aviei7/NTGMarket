using Application.Common.Cache;
using Application.DTO.Input;
using Application.DTO.Output;
using Application.DTO.Output.FilterDTO;
using Application.DTO.Output.ProductDTO;
using Application.Interfaces;
using Application.Interfaces.Cache;
using Application.Interfaces.Catalog;
using Application.Interfaces.CatalogFilter;
using Application.Interfaces.DBContext;
using Application.Services.Pagination;
using Domain.Models.FiltersModel;
using Domain.Models.ProductsModel;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogReprository _CatalogReprository;
        private readonly IFilterRepository _FilterRepository;
        private readonly IFilterService _FilterService;
        private readonly IPaginationService _PaginationService;
        private readonly IBoardStoreContext _DBContext;


        /*Redis*/
        private readonly ICacheService _cacheService;


        public CatalogService(ICatalogReprository CatalogReprository, IFilterService FilterService, IFilterRepository FilterRepository, IPaginationService PaginationService, ICacheService cacheService, IBoardStoreContext DBContext)
        {
            _CatalogReprository = CatalogReprository;
            _FilterRepository = FilterRepository;
            _FilterService = FilterService;
            _PaginationService = PaginationService;
            _DBContext = DBContext;
            _cacheService = cacheService;
        }

        /*Возврат списка товаров с фильтрацией, сортировкой и пагинацией(рабиение на страницы)*/
        public async Task<PageResultDto<ProductListDto>> GetProductsAsync(ProductQuery pq, CancellationToken ct)
        {
            IQueryable<ProductModel> query = _DBContext.Products
                .AsNoTracking()
                .Where(p => p.IsActive);
            IReadOnlyList<ProductListDto> items = new List<ProductListDto>();
            decimal MinPrice = 0;
            decimal MaxPrice = 0;
            query = _FilterService.GetFilteredProduct(pq, query);

   

            var TotalCount = await query.CountAsync(ct);

            if (TotalCount > 0)
            {
                MinPrice = await query.MinAsync(p => p.Price, ct);
                MaxPrice = await query.MaxAsync(p => p.Price, ct);


                var primaryImages = _DBContext.ProductImages
                    .AsNoTracking()
                    .Where(i => i.IsPrimary);

                var projectedQuery =
                    from p in query
                    join image in primaryImages on p.ID equals image.ProductId into productImages
                    select new ProductListDto
                    {
                        Id = p.ID,
                        Name = p.Name,
                        Slug = p.Slug,
                        Price = p.Price,
                        Stock = p.Stock,
                        CategoryId = p.CategoryID,
                        CategoryName = p.Category.CategoryName,
                        Description = p.Description,
                        PrimaryImageUrl = productImages
                            .Select(i => i.FileName)
                            .FirstOrDefault()
                    };

                items = await _PaginationService.PaginationResult(projectedQuery, pq, ct);

            }

            return new PageResultDto<ProductListDto>
            {
                Items = items,
                TotalCount = TotalCount,
                Page = pq.Page,
                PageSize = pq.PageSize,
                HasMoreItems = TotalCount > pq.Page * pq.PageSize,
                MinPrice = MinPrice,
                MaxPrice = MaxPrice
            };
        }

        /*Возврат подробной информацию о конкретном товаре*/
        public async Task<ProductDetailsDto?> GetProductAsync(string idOrSlug, CancellationToken ct)
        {
            IQueryable<ProductModel> query = _CatalogReprository.SelectProducts();

            query = query.Where(p => p.IsActive == true);

            if (int.TryParse(idOrSlug, out int id))
            {
                query = query.Where(p => p.ID == id);
            }
            else
            {
                var slug = idOrSlug.ToLowerInvariant().Trim();
                query = query.Where(p => p.Slug.ToLower() == slug);
            }

            return await query.Select(p => new ProductDetailsDto { ID = p.ID, Name = p.Name, Slug = p.Slug, Price = p.Price, CategoryID = p.CategoryID, Description = p.Description/*, Images = p.Images*/ })
                                    .FirstOrDefaultAsync(ct);
        }
        /*Возврат списка категорий для фильтров/меню*/
        public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken ct)
        {

            IQueryable<f_CategoryModel> query = _CatalogReprository.SelectCategory();

            var categories = await _cacheService.GetOrSetAsync(
                                                 CacheKeys.CategoriesKey,
                                                 () => query.OrderBy(c => c.CategoryID)
                                                      .Select(c => new CategoryDto { CategoryID = c.CategoryID, 
                                                                                     Name = c.CategoryName, 
                                                                                     Slug = c.CategorySlug })
                                                      .ToListAsync(ct),
                                                 CacheTTL.Categories, 
                                                 ct);

            return categories;
        }

    }
}
