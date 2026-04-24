using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdersModels
{
    public class DeliveryLogModel
    {
        [Key]
        public int DeliveryLogID { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int DeliveryMethod { get; set; }
        public DateTime SendDate { get; set; }

        public OrdersModel? Order { get; set; }
    }
}
