using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.LoyaltyModel
{
    public class DiscountModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal DiscountPercent { get; set; }
        public string PromoCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
