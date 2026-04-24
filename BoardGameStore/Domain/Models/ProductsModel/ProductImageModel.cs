namespace Domain.Models.ProductsModel
{
    public class ProductImageModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }
}
