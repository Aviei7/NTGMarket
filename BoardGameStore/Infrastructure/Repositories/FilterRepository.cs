using Application.Interfaces.CatalogFilter;
using Azure;
using Domain.Models.FiltersModel;
using Domain.Models.StaticModel;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public class FilterRepository: IFilterRepository
    {
        private readonly BoardStoreContext _context;
        public FilterRepository(BoardStoreContext context)
        {
            _context = context;
        }
        public f_FilterInfoModel? GetOneFilters(string queryParam)
        {
            Console.WriteLine($"FieldName: {queryParam}");



            //return _context.f_Filters.Include(f => f.Filter_QueryParam).AsNoTracking().FirstOrDefault(f => f.Filter_QueryParam.Any(o => o.QueryParam == queryParam));

            var query = _context.f_FilterOperations.Include(o => o.FilterInfo).AsNoTracking().Where(o => o.QueryParam == queryParam).Select(o => new f_FilterInfoModel {
                FilterID = o.FilterInfo.FilterID,
                FieldName = o.FilterInfo.FieldName,
                FilterName = o.FilterInfo.FilterName,
                FilterTypeID = o.FilterInfo.FilterTypeID,
                TableID = o.FilterInfo.TableID,
                Filter_QueryParam = new List<f_FilterOperationModel> { o }});

            Console.WriteLine("=== GetOneFilters ===");
            Console.WriteLine(query.ToQueryString());

            return query.FirstOrDefault(); ;

        }

        public IQueryable<f_FilterInfoModel> GetFiltersForTable(string tableName)
        {
           return _context.f_Filters.AsNoTracking().Where(f => f.TableInfo.TableName == tableName).Include(f => f.Filter_QueryParam).Include(f => f.FilterInfo);
        }

        public f_FilterOperationModel? GetOperators(string QueryParam)
        {

            Console.WriteLine($"FieldName: {QueryParam}");
            var query = _context.f_FilterOperations.Include(f => f.OperationInfo).AsNoTracking().Where(f => f.QueryParam == QueryParam); ;

            Console.WriteLine("=== GetOperators ===");
            Console.WriteLine(query.ToQueryString());

            return _context.f_FilterOperations.Include(f => f.OperationInfo).AsNoTracking().FirstOrDefault(f => f.QueryParam == QueryParam);
        }


        public f_FilterOperationModel GetOperationValueName(int value)
        {
            return _context.f_FilterOperations.AsNoTracking().FirstOrDefault(f => f.ID == value);
        }

        public f_FilterOperationModel? GetMaxAndMinPrice(string QueryParam)
        {
            return _context.f_FilterOperations.AsNoTracking().FirstOrDefault(f => f.QueryParam == QueryParam);
        }
    }
}
