using Domain.Models.ProductsModel;
using System.Linq;

namespace Application.Interfaces.Cart
{
    public interface ICartRepository
    {
        IQueryable<ProductModel> GetProducts();
    }
}
