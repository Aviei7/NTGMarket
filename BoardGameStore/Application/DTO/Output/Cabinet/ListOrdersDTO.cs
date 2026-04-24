using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Output.Cabinet
{
    public class ListOrdersDTO
    {
        public int OrderID { get; set; }
        public DateOnly OrderDate { get; set; }
        public Decimal OrderSum { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
    }
}
