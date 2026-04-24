using Domain.Models.LoyaltyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Loyalty
{
    public interface ILoyaltyRepository
    {
        public Task<DiscountModel?> GetDiscountByPromoCode(string promoCode);
    }
}
