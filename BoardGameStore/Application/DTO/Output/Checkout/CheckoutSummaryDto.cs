namespace Application.DTO.Output.Checkout
{
    public class CheckoutSummaryDto
    {
        public int ItemsCount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DeliveryAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
