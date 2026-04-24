using Application.DTO.Input.Checkout;
using Application.DTO.Output.Checkout;

namespace Application.Interfaces.Checkout
{
    public interface ICheckoutService
    {
        public Task<CheckoutInfoDto> GetCheckoutInfo();
        Task<CheckoutSummaryDto> GetSummary();
        Task<CheckoutValidationResultDto> ValidateCheckout(CheckoutValidateDto dto);
        Task<CheckoutSubmitResultDto> SubmitCheckout(CheckoutSubmitDto dto);
    }
}
