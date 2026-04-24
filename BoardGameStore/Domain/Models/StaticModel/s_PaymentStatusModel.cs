using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.OrdersModels;

namespace Domain.Models.StaticModel
{
    public class s_PaymentStatusModel
    {
        [Key]
        public int PaymentStatusId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Status { get; set; }

        public ICollection<OrdersModel> Orders { get; set; } = new List<OrdersModel>();
    }
}
