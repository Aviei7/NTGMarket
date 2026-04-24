namespace Application.DTO.Output.Checkout
{
    public class CheckoutValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
