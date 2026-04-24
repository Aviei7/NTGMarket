using Application.DTO.Output.Loyalty;
using Application.Interfaces.DBContext;
using Application.Interfaces.Loyalty;

namespace Application.Services.Loyalty
{
    public class DiscountService : IDiscountService
    {
        private readonly ILoyaltyRepository _loyaltyRepository;
        public DiscountService(ILoyaltyRepository loyaltyRepository) 
        {
            _loyaltyRepository = loyaltyRepository;
        }    

        public async Task<DiscountByPromoDto?> GetDiscountByPromoCode(string promoCode)
        {
            var discountModel = await _loyaltyRepository.GetDiscountByPromoCode(promoCode);

            if (discountModel is null)
            {
                return null;
            }

            return new DiscountByPromoDto
            {
                Id = discountModel.Id,
                PromoCode = discountModel.PromoCode,
                DiscountPercentage = discountModel.DiscountPercent,
            };
        }


    }
}
