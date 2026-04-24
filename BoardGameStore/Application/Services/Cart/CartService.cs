using Application.Common.Cache;
using Application.Common.Cart;
using Application.DTO.Output.Cart;
using Application.Interfaces.Auth;
using Application.Interfaces.Cache;
using Application.Interfaces.Cart;
using Application.Interfaces.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ICacheService _cacheService;
        private readonly ICookieService _cookieService;
        private readonly CartCookieOptions _cartOptions;
        private readonly ICartTokenService _cartTokenService;
        private readonly ICartRepository _cartRepository;

        public CartService(
            ICacheService cacheService,
            ICookieService cookieService,
            ICartTokenService cartTokenService,
            ICartRepository cartRepository,
            IOptions<CartCookieOptions> cartOptions)
        {
            _cacheService = cacheService;
            _cookieService = cookieService;
            _cartTokenService = cartTokenService;
            _cartRepository = cartRepository;
            _cartOptions = cartOptions.Value;
        }

        public async Task<CartViewDto> AddInCart(int itemID)
        {
            var session = GetOrCreateCartSession();
            var cart = await GetStorageCart(session.CartToken);

            var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == itemID);

            if (existingItem is null)
            {
                cart.Items.Add(new CartStorageItemDto
                {
                    ProductId = itemID,
                    Quantity = 1
                });
            }
            else
            {
                existingItem.Quantity++;
            }

            await SaveCart(cart, session);

            return await GetCart();
        }


        public async Task<CartViewDto> GetCart()
        {
            var cartToken = _cookieService.GetTokenFromRequest(_cartOptions.TokenName);
            if (string.IsNullOrWhiteSpace(cartToken))
            {
                return new CartViewDto();
            }

            var cart = await GetStorageCart(cartToken);

            if (cart is null || !cart.Items.Any())
            {
                return new CartViewDto();
            }

            return await CreateResponseCart(cart);
        }

        public async Task<CartViewDto> AddItem(int itemId)
        {
            var session = GetOrCreateCartSession();

            var cart = await GetStorageCart(session.CartToken);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == itemId);
            if (item is null)
                return await GetCart();

            item.Quantity++;

            await SaveCart(cart, session);

            return await GetCart();
        }

        public async Task<CartViewDto> SubItem(int itemId)
        {
            var session = GetOrCreateCartSession();

            var cart = await GetStorageCart(session.CartToken);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == itemId);
            if (item is null)
                return await GetCart();

            item.Quantity--;

            if (item.Quantity <= 0)
            {
                cart.Items.Remove(item);
            }

            await SaveCart(cart, session);

            return await GetCart();
        }

        public async Task<CartViewDto> RemoveItem(int itemId)
        {
            var session = GetOrCreateCartSession();
            var cart = await GetStorageCart(session.CartToken);
            var item = cart.Items.FirstOrDefault(x => x.ProductId == itemId);
            if (item is null)
                return await GetCart();
            cart.Items.Remove(item);
            await SaveCart(cart, session);
            return await GetCart();
        }

        public async Task<CartViewDto> ClearCart()
        {
            var session = GetOrCreateCartSession();
            var cart = new CartStorageDto
            {
                Items = new List<CartStorageItemDto>()
            };
            await SaveCart(cart, session);
            return new CartViewDto();
        }

        //Private methods

        private async Task<CartViewDto> CreateResponseCart(CartStorageDto cart)
        {
            var productIds = cart.Items
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = await _cartRepository.GetProducts()
                .Where(p => productIds.Contains(p.ID) && p.IsActive)
                .ToListAsync();

            var productsById = products.ToDictionary(p => p.ID);
            var viewItems = new List<CartViewItemDto>();

            foreach (var cartItem in cart.Items)
            {
                if (!productsById.TryGetValue(cartItem.ProductId, out var product))
                {
                    continue;
                }

                viewItems.Add(new CartViewItemDto
                {
                    ProductId = product.ID,
                    ProductName = product.Name ?? string.Empty,
                    UnitPrice = product.Price,
                    Quantity = cartItem.Quantity,
                    SumPrice = product.Price * cartItem.Quantity,
                    ImageUrl = product.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.FileName)
                        .FirstOrDefault()
                });
            }

            return new CartViewDto
            {
                Items = viewItems,
                TotalQuantity = viewItems.Sum(x => x.Quantity),
                TotalPrice = viewItems.Sum(x => x.SumPrice)
            };
        }

        private async Task<CartStorageDto> GetStorageCart(string cartToken)
        {
            var cartCacheKey = CacheKeys.CartKeyToken(cartToken);
            return await _cacheService.GetAsync<CartStorageDto>(cartCacheKey) ?? new CartStorageDto();
        }

        private async Task SaveCart(CartStorageDto cart, CartSession session)
        {
            var cartCacheKey = CacheKeys.CartKeyToken(session.CartToken);
            await _cacheService.SetAsync(cartCacheKey, cart, session.CartTtl);
        }

        private CartSession GetOrCreateCartSession()
        {
            var cartToken = _cookieService.GetTokenFromRequest(_cartOptions.TokenName);
            var cartTtl = _cartTokenService.GetRemainingCartTokenTtl(cartToken);

            if (!string.IsNullOrWhiteSpace(cartToken) && cartTtl > TimeSpan.Zero)
            {
                return new CartSession(cartToken, cartTtl);
            }

            var expiresAtUtc = DateTimeOffset.UtcNow.AddDays(_cartOptions.ExpireDays);
            cartToken = _cartTokenService.CreateCartToken(expiresAtUtc);
            cartTtl = expiresAtUtc - DateTimeOffset.UtcNow;

            _cookieService.SetJwtCookie(
                cartToken,
                _cartTokenService.BuildCartCookieOptions(expiresAtUtc),
                _cartOptions.TokenName);

            return new CartSession(cartToken, cartTtl);
        }

        private readonly record struct CartSession(string CartToken, TimeSpan CartTtl);

        //        AddItem
        //RemoveItem
        //GetCart
        //ClearCart



    }
}
