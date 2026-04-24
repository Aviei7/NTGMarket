using Application.DTO.Input;
using Application.DTO.Output.FilterDTO;
using Domain.Models.ProductsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CatalogFilter
{
    public interface IFilterService
    {
        public IQueryable<ProductModel> GetFilteredProduct(ProductQuery pq, IQueryable<ProductModel> query);
        public Task<IReadOnlyList<FilterListDto>> GetFiltersList(CancellationToken ct);
    }
}
