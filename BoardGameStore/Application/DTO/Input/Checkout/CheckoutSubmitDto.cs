namespace Application.DTO.Input.Checkout
{
    public class CheckoutSubmitDto
    {
        /*Users*/
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        /*Delivery*/
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int DeliveryMethod { get; set; } 
        /*Payment*/
        public int PaymentMethod { get; set; } 
        public int PaymentStatusId { get; set; }

        /*Other*/
        public Decimal TotalPrice { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
