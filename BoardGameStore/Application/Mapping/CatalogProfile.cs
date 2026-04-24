using AutoMapper;
using Application.DTO.Output.FilterDTO;
using Application.DTO.Output.ProductDTO;
using Domain.Models.FiltersModel;
using Domain.Models.ProductsModel;

namespace Application.Mapping
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<f_CategoryModel, CategoryDto>();
            CreateMap<ProductImageModel, ProductImageDto>()
                .ForCtorParam("Url", x => x.MapFrom(p => p.FileName));
            CreateMap<ReviewModel, CategoryDto>();
            CreateMap<ProductModel, ProductListDto>()
                .ForCtorParam("CategoryName", x => x.MapFrom(p => p.Category.CategoryName))
                .ForCtorParam("PrimaryImageUrl", x => x.MapFrom(p => p.Images.Where(i => i.IsPrimary).Select(i => i.FileName).FirstOrDefault()))
                .ForCtorParam("AvgRating", x => x.MapFrom(p => p.Review.Where(r => r.IsVisible).Select(r => (double?)r.Rating).DefaultIfEmpty().Average()));

            CreateMap<ProductModel, ProductDetailsDto>()
                .ForCtorParam("AvgRating", x => x.MapFrom(p => p.Review.Where(r => r.IsVisible).Select(r => (double?)r.Rating).DefaultIfEmpty().Average()));
        }
    }
}
