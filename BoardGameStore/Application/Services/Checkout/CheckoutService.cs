using Application.DTO.Input.Checkout;
using Application.DTO.Output.Checkout;
using Application.Exceptions;
using Application.Interfaces.Checkout;
using Application.Interfaces.Users;

namespace Application.Services.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IAuthValidationService _authValidationService;

        public CheckoutService(
            ICheckoutRepository checkoutRepository,
            IUsersRepository usersRepository,
            IAuthValidationService authValidationService)
        {
            _checkoutRepository = checkoutRepository;
            _usersRepository = usersRepository;
            _authValidationService = authValidationService;
        }

        public async Task<CheckoutInfoDto> GetCheckoutInfo()
        {
            var deliveryMethods = await _checkoutRepository.GetDeliveryMethods();
            var paymentMethods = await _checkoutRepository.GetPaymentMethods();

            return new CheckoutInfoDto
            {
                DeliveryMethods = deliveryMethods,
                PaymentMethods = paymentMethods
            };
        }

        public async Task<CheckoutSummaryDto> GetSummary()
        {
            return await _checkoutRepository.GetSummary();
        }

        public async Task<CheckoutValidationResultDto> ValidateCheckout(CheckoutValidateDto dto)
        {
            var errors = new List<string>();

            try
            {
                _authValidationService.ValidateEmail(dto.Email);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex.UserMessage);
            }

            try
            {
                _authValidationService.ValidatePhone(dto.Phone);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex.UserMessage);
            }

            return await Task.FromResult(new CheckoutValidationResultDto
            {
                IsValid = errors.Count == 0,
                Errors = errors
            });
        }

        public async Task<CheckoutSubmitResultDto> SubmitCheckout(CheckoutSubmitDto dto)
        {
            var user = await _usersRepository.GetByEmail(dto.Email);

            var userId = user?.UserID ?? await _usersRepository.GetGuestUserID();

            var order = await _checkoutRepository.SubmitCheckout(dto, userId);

            return new CheckoutSubmitResultDto
            {
                OrderId = order.Id,
                Status = "Success",
                Message = "Checkout submitted successfully."
            };
        }
    }
}
