using Domain.Models.ProductsModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.FiltersModel
{
    public class f_CategoryModel
    {
        [Key]
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? CategorySlug { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
