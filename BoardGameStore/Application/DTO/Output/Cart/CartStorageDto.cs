using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Output.Cart
{
    public class CartStorageDto
    {
        public List<CartStorageItemDto> Items { get; set; } = new();
    }
}
