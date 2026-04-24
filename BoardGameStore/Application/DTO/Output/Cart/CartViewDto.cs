using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Output.Cart
{
    public class CartViewDto
    {
        public List<CartViewItemDto> Items { get; set; } = new();
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
