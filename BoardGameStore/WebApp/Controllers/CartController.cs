using Application.DTO.Output.Cart;
using Application.Interfaces.Cache;
using Application.Interfaces.Cart;
using Application.Services.Cart;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("addInCart/{prodID}")]
        public async Task<ActionResult<CartViewDto>> AddInCart(int prodID)
        {
            return Ok(await _cartService.AddInCart(prodID));
        }

        [HttpGet("cart")]
        public async Task<CartViewDto> GetCartInfo()
        {
            return await _cartService.GetCart();
        }

        [HttpGet("subItem/{prodId}")]
        public async Task<CartViewDto> SubCartItem(int prodId)
        {
            return await _cartService.SubItem(prodId);
        }

        [HttpGet("addItem/{prodId}")]
        public async Task<CartViewDto> AddCartItem(int prodId)
        {
            return await _cartService.AddItem(prodId);
        }

        [HttpGet("removeItem/{prodId}")]
        public async Task<CartViewDto> RemoveCartItem(int prodId)
        {
            return await _cartService.RemoveItem(prodId);
        }

        [HttpGet("clearCart")]
        public async Task<CartViewDto> ClearCart()
        {
            return await _cartService.ClearCart();
        }

    }

}
