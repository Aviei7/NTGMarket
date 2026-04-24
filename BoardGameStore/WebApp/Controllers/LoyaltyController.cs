using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Loyalty;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoyaltyController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public LoyaltyController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet("GetDiscountByPromoCode")]
        public async Task<IActionResult> GetDiscountByPromoCode(string promoCode)
        {
            var discount = await _discountService.GetDiscountByPromoCode(promoCode);

            if (discount is null)
            {
                return NotFound(new { message = "Promo code not found" });
            }

            return Ok(discount);
        }
    }
}
