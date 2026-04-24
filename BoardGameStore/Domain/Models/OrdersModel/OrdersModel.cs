using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.StaticModel;
using Domain.Models.UsersModel;

namespace Domain.Models.OrdersModels
{
    public class OrdersModel
    {
        [Key]
        public int Id { get; set; }
        public int UserOrderLogID { get; set; }
        public int DeliveryLogID { get; set; }
        public int PaymentTypeId { get; set; }
        public int PaymentStatusId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserOrderLogModel UserOrderLog { get; set; } = null!;
        public DeliveryLogModel DeliveryLog { get; set; } = null!;
        public s_PaymentTypesModel PaymentType { get; set; } = null!;
        public s_PaymentStatusModel PaymentStatus { get; set; } = null!;
    }
}
