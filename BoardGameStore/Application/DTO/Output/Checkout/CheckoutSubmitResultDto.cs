namespace Application.DTO.Output.Checkout
{
    public class CheckoutSubmitResultDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
