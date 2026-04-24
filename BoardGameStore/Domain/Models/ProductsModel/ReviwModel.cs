namespace Domain.Models.ProductsModel
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; } = null!;
        public int UserId { get; set; }      // позже свяжем с User
        public byte Rating { get; set; }     // 1..5
        public string? Text { get; set; }
        public bool IsVisible { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
