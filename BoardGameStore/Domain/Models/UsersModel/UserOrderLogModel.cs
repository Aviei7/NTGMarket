using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.OrdersModels;

namespace Domain.Models.UsersModel
{
    public class UserOrderLogModel
    {
        [Key]
        public int UserOrderLogID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public OrdersModel? Order { get; set; }
    }
}
