using Application.DTO.Input.Checkout;
using Application.DTO.Output.Checkout;
using Domain.Models.OrdersModels;
using Domain.Models.StaticModel;

namespace Application.Interfaces.Checkout
{
    public interface ICheckoutRepository
    {
        public Task<List<s_DeliveryTypesModel>> GetDeliveryMethods();
        public Task<List<s_PaymentTypesModel>> GetPaymentMethods();
        public Task<CheckoutSummaryDto> GetSummary();
        public Task<CheckoutValidationResultDto> ValidateCheckout(CheckoutValidateDto dto);
        public Task<OrdersModel> SubmitCheckout(CheckoutSubmitDto dto, int userID);
    }
}
