using System.ComponentModel.DataAnnotations;
using Domain.Models.OrdersModels;

namespace Domain.Models.StaticModel
{
    public class s_PaymentTypesModel
    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<OrdersModel> Orders { get; set; } = new List<OrdersModel>();
    }
}
