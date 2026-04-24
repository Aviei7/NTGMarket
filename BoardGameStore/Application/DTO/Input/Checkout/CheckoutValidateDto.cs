namespace Application.DTO.Input.Checkout
{
    public class CheckoutValidateDto
    {
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
