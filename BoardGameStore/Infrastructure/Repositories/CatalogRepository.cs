using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Models.FiltersModel;
using Domain.Models.ProductsModel;
using Application.Interfaces.Catalog;


namespace Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogReprository
    {
        private readonly BoardStoreContext _context;

        public CatalogRepository(BoardStoreContext context)
        {
            _context = context;
        }

        /*Возврат списка товаров с фильтрацией, сортировкой*/
        public IQueryable<ProductModel> SelectProducts()
        {
            return _context.Products.AsNoTracking().Where(p => p.IsActive);
        }

        public IQueryable<f_CategoryModel> SelectCategory()
        {
            return _context.f_Categories.AsNoTracking();
        }
    }
}
