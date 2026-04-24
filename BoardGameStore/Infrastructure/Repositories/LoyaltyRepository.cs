using Application.DTO.Output.Loyalty;
using Application.Interfaces.DBContext;
using Application.Interfaces.Loyalty;
using Domain.Models.LoyaltyModel;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LoyaltyRepository : ILoyaltyRepository
    {
        private readonly IBoardStoreContext _context;
        public LoyaltyRepository(IBoardStoreContext boardStoreContext) 
        {
            _context = boardStoreContext;
        }

        public async Task<DiscountModel?> GetDiscountByPromoCode(string promoCode)
        {
            return await _context.l_Dsicounts.AsNoTracking().FirstOrDefaultAsync(p => p.PromoCode == promoCode && p.IsActive == true);

        }
    }
}
