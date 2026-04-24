using Domain.Models.StaticModel;
using System.Collections.Generic;

namespace Application.DTO.Output.Checkout
{
    public class CheckoutInfoDto
    {
        public List<s_DeliveryTypesModel> DeliveryMethods { get; set; } = new();
        public List<s_PaymentTypesModel> PaymentMethods { get; set; } = new();
    }
}
