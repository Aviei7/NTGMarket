using Domain.Models.FiltersModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CatalogFilter
{
    public interface IFilterRepository 
    {
        public f_FilterInfoModel? GetOneFilters(string queryParam);
        public IQueryable<f_FilterInfoModel> GetFiltersForTable(string tableName);
        public f_FilterOperationModel? GetOperators(string queryParam);
        public f_FilterOperationModel GetOperationValueName(int value);
    }
}
