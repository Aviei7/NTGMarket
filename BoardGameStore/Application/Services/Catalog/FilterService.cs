using Application.DTO.Input;
using Application.DTO.Output;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.DTO.Output.FilterDTO;
using Domain.Models.FiltersModel;
using Domain.Models.ProductsModel;
using Application.Interfaces.CatalogFilter;
using Application.Interfaces.Catalog;

namespace Application.Services.Catalog
{
    public class FilterService : IFilterService
    {

        private readonly IFilterRepository _FilterRepository;
        private readonly ICatalogReprository _CatalogReprository;

        public FilterService(IFilterRepository FilterRepository, ICatalogReprository CatalogReprository)
        {
            _CatalogReprository = CatalogReprository;
            _FilterRepository = FilterRepository;
        }


        public IQueryable<ProductModel> GetFilteredProduct(ProductQuery pq, IQueryable<ProductModel> query)
        {

            if (pq.CategoryId.HasValue && pq.CategoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryID == pq.CategoryId);
            }



            if (pq.AllFilter is not null)
            {
                query = ApplyFilter(pq.AllFilter, query);
    
            }

            query = ApplySort(query, pq.Sort);
            Console.WriteLine("===============ENDGetFilteredProduct===============");
            return query;

        }

        public async Task<IReadOnlyList<FilterListDto>> GetFiltersList(CancellationToken ct)
        {
            IQueryable<f_FilterInfoModel> query = _FilterRepository.GetFiltersForTable("Products");

            return await query.Select(c => new FilterListDto 
                              { ID = c.FilterID, FilterName = c.FilterName, FieldName = c.FieldName, FilterType = c.FilterInfo.ComponentName, ParamList = c.Filter_QueryParam
                                .Select(f => new QueryParamDto 
                                { ParamID = f.ID, QueryParam = f.QueryParam, OperationName = f.OperationName}).ToList()
                              }).ToListAsync(ct);
        }


        private IQueryable<ProductModel> ApplyFilter(ICollection<FilterValueDto> AllFilter, IQueryable<ProductModel> query)
        {
            // 1) Группируем заранее
            var groups = (AllFilter ?? Enumerable.Empty<FilterValueDto>()).GroupBy(f => f.FieldName);

            // 2) Применяем по группам: внутри группы — OR, между группами — AND
            foreach (var g in groups)
            {
                // Берём meta/operators один раз на поле
                var first = g.First();
                var meta = _FilterRepository.GetOneFilters(first.QueryParam);
                var operators = _FilterRepository.GetOperators(first.QueryParam);

                Console.WriteLine($"Meta: {(meta == null ? "null" : meta.FieldName)}, Operators: {(operators == null ? "null" : operators.OperationID.ToString())}, query: {query}, value: {groups} ");

                if (meta is null || operators is null) continue;

                // Шаблон выражения для одного значения
                // напр.: "{field}.Contains(@0)" → "Brand.Contains(@0)"
                var singleTemplate = operators.OperationInfo.ExpressionTemplate
                    .Replace("{field}", meta.FieldName);

                if (g.Count() == 1)
                {
                    // Одиночный фильтр — как и раньше
                    var single = g.First();
                    var valObj = _FilterRepository.GetOperationValueName(single.FilterValue);
                    var arg = ExtractArg(valObj); // см. функцию ниже
                                                  // Если у тебя ApplyFilter принимает int — либо перегрузи, либо вызывай напрямую Where:
                                                  // query = query.Where(singleTemplate, arg);
                    query = query.Where(singleTemplate, arg);
                    continue;
                }

                // Несколько значений по одному полю → строим OR с индексами @0, @1, ...
                var orExpr = string.Join(" OR ",
                    g.Select((it, i) => singleTemplate.Replace("@0", $"@{i}")));

                var args = g
                    .Select(it => _FilterRepository.GetOperationValueName(it.FilterValue))
                    .Select(ExtractArg)
                    .Cast<object>()
                    .ToArray();

                // Скобки обязательны, чтобы не сломать приоритет с другими условиями (IsActive и т.п.)
                query = query.Where("(" + orExpr + ")", args);
            }
            Console.WriteLine("ApplyFilterQuery");
            Console.WriteLine(query.ToQueryString());
            return query;
        }
            private static object ExtractArg(object valObj)
            {
                // Если у тебя объект с полем OperationName — берём строку.
                // Иначе верни само значение/ToString().
                var prop = valObj?.GetType().GetProperty("OperationName");
                if (prop != null) return prop.GetValue(valObj) ?? "";
                return valObj?.ToString() ?? "";
            }

        private IQueryable<ProductModel> ApplySort(IQueryable<ProductModel> query, string? sort)
        {
            return sort switch
            {
                "NameAsc" => query.OrderBy(p => p.Name),
                "NameDesc" => query.OrderByDescending(p => p.Name),
                "PriceAsc" => query.OrderBy(p => p.Price),
                "PriceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.ID)
            };
        }
    }

}
