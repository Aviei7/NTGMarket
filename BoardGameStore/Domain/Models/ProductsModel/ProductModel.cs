using Domain.Models.FiltersModel;

namespace Domain.Models.ProductsModel
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryID { get; set; }
        public f_CategoryModel? Category { get; set; }
        public string Brand { get; set; }
        public string? Description { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public ICollection<ProductImageModel> Images { get; set; } = new List<ProductImageModel>();
        public ICollection<ReviewModel> Review { get; set; } = new List<ReviewModel>();
    }
}
