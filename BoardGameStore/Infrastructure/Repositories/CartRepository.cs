using Application.Interfaces.Cart;
using Domain.Models.ProductsModel;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BoardStoreContext _context;

        public CartRepository(BoardStoreContext context)
        {
            _context = context;
        }

        public IQueryable<ProductModel> GetProducts()
        {
            return _context.Products
                .AsNoTracking()
                .Include(p => p.Images);
        }
    }
}
