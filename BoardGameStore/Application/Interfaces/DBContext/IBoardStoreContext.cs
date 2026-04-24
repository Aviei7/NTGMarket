using Domain.Models.FiltersModel;
using Domain.Models.LoyaltyModel;
using Domain.Models.OrdersModels;
using Domain.Models.ProductsModel;
using Domain.Models.StaticModel;
using Domain.Models.UserModel;
using Domain.Models.UsersModel;
using Microsoft.EntityFrameworkCore;


namespace Application.Interfaces.DBContext
{
    public interface IBoardStoreContext
    {
        
        /*Product*/
        public DbSet<f_CategoryModel> f_Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        /*Filters*/
        public DbSet<f_FilterInfoModel> f_Filters { get; set; }
        public DbSet<f_FilterOperationModel> f_FilterOperations { get; set; }
        /*Static*/
        public DbSet<s_TablesModel> s_Tables { get; set; }
        public DbSet<s_OperationModel> s_Operations { get; set; }
        public DbSet<s_FilterTypeModel> s_FilterTypes { get; set; }
        public DbSet<s_DeliveryTypesModel> s_DeliveryTypes { get; set; }
        public DbSet<s_PaymentTypesModel> s_PaymentTypes { get; set; }
        public DbSet<s_PaymentStatusModel> s_PaymentStatus { get; set; }
        /*Users*/
        public DbSet<UsersModel> Users { get; set; }
        /*Loyalty*/
        public DbSet<DiscountModel> l_Dsicounts { get; set; }
        public DbSet<OrdersModel> Orders { get; set; }
        public DbSet<UserOrderLogModel> UserLogs { get; set; }
        public DbSet<DeliveryLogModel> DeliveryLogs { get; set; }
    }
}
