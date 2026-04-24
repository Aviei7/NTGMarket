using Application.DTO.Input;
using Application.DTO.Output;
using Application.DTO.Output.FilterDTO;
using Application.DTO.Output.ProductDTO;
using Application.Interfaces;
using Application.Interfaces.CatalogFilter;
using Infrastucture.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BoardStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IFilterService _filterService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(BoardStoreContext DbContext, ICatalogService CatContext, IFilterService FilContext, ILogger<CatalogController> logger)
        {
            _catalogService = CatContext;
            _filterService = FilContext;
            _logger = logger;
        }

        [HttpGet("category")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCatalogList(CancellationToken ct)
        {
            var result = await _catalogService.GetCategoriesAsync(ct);
            if (result == null || !result.Any())
                throw new KeyNotFoundException();
            return Ok(result);

        }
        [HttpGet("filterlist")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetFilterForProduct(CancellationToken ct)
        {
            var result = await _filterService.GetFiltersList(ct);
            if (result == null || !result.Any())
                throw new KeyNotFoundException();
            return Ok(result);

        }

        [HttpPost("products/filtered")]
        public async Task<ActionResult<PageResultDto<ProductListDto>>> PostFilteredProductList([FromBody] ProductQuery pq, CancellationToken ct)
        {
            Console.WriteLine($"✅ JSON log saved to ");

            var result = await _catalogService.GetProductsAsync(pq, ct);

            return Ok(result);
        }

        [HttpGet("products")]
        //[Authorize]
        public async Task<ActionResult<PageResultDto<ProductListDto>>> GetProductList([FromQuery] ProductQuery pq, CancellationToken ct)
        {
            var result = await _catalogService.GetProductsAsync(pq, ct);

            return Ok(result);
        }


        [HttpGet("products/{idOrSlug}")]
        public async Task<ActionResult<ProductDetailsDto?>> GetProductFromIDAsync(string idOrSlug, CancellationToken ct)
        {
            var result = await _catalogService.GetProductAsync(idOrSlug, ct);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                return Ok(result);
            }

        }
    }
}
