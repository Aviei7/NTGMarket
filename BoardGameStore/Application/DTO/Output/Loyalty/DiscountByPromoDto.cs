using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Output.Loyalty
{
    public class DiscountByPromoDto
    {
        public int Id { get; set; }
        public string? PromoCode { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
