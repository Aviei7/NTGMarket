using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Output.Cart;

namespace Application.Interfaces.Cart
{
    public interface ICartService
    {
        public Task<CartViewDto> AddInCart(int itemID);
        public Task<CartViewDto> GetCart();
        public Task<CartViewDto> SubItem(int itemId);
        public Task<CartViewDto> AddItem(int itemId);
        public Task<CartViewDto> ClearCart();
        public Task<CartViewDto> RemoveItem(int itemId);
    }
}
