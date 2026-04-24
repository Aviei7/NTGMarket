using Application.DTO.Input.Checkout;
using Application.DTO.Output.Checkout;
using Application.Interfaces.Checkout;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpGet("info")]
        [HttpGet("checkoutInfo")]
        public async Task<ActionResult<CheckoutInfoDto>> GetCheckoutInfo()
        {
            var info = await _checkoutService.GetCheckoutInfo();
            return Ok(info);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<CheckoutSummaryDto>> GetSummary()
        {
            return Ok(await _checkoutService.GetSummary());
        }

        [HttpPost("validate")]
        public async Task<ActionResult<CheckoutValidationResultDto>> ValidateCheckout([FromBody] CheckoutValidateDto dto)
        {
            return Ok(await _checkoutService.ValidateCheckout(dto));
        }

        [HttpPost("submit")]
        public async Task<ActionResult<CheckoutSubmitResultDto>> SubmitCheckout([FromBody] CheckoutSubmitDto dto)
        {
            return Ok(await _checkoutService.SubmitCheckout(dto));
        }
    }
}
