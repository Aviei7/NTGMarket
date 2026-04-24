using Application.Interfaces.DBContext;
using Domain.Models.FiltersModel;
using Domain.Models.OrdersModels;
using Domain.Models.ProductsModel;
using Domain.Models.StaticModel;
using Domain.Models.UserModel;
using Domain.Models.LoyaltyModel;
using Microsoft.EntityFrameworkCore;
using Domain.Models.UsersModel;

namespace Infrastucture.Context
{
    public class BoardStoreContext : DbContext, IBoardStoreContext
    {
        public BoardStoreContext(DbContextOptions<BoardStoreContext> options) : base(options)
        {
        }

        /*Product*/
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        /*Filters*/
        public DbSet<f_CategoryModel> f_Categories { get; set; }
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
        /*Orders*/
        public DbSet<OrdersModel> Orders { get; set; }
        public DbSet<UserOrderLogModel> UserLogs { get; set; }
        public DbSet<DeliveryLogModel> DeliveryLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<f_CategoryModel>(e =>
            {
                e.Property(x => x.CategoryName).HasMaxLength(100).IsRequired();
                e.Property(x => x.CategorySlug).HasMaxLength(120).IsRequired();
                e.HasIndex(x => x.CategorySlug).IsUnique();
            });

            modelBuilder.Entity<ProductModel>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.Property(x => x.Slug).HasMaxLength(100).IsRequired();
                e.HasIndex(x => x.Slug).IsUnique();
                e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ProductImageModel>(e =>
            {
                e.Property(x => x.FileName).HasMaxLength(255).IsRequired();
                e.HasIndex(x => new { x.ProductId, x.IsPrimary });
            });

            modelBuilder.Entity<ReviewModel>(e =>
            {
                e.HasIndex(x => new { x.ProductId, x.UserId }).IsUnique();
            });

            modelBuilder.Entity<s_DeliveryTypesModel>(e =>
            {
                e.HasKey(x => x.DeliveryId);
                e.Property(x => x.DeliveryName).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<s_PaymentTypesModel>(e =>
            {
                e.HasKey(x => x.PaymentId);
                e.Property(x => x.PaymentName).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<s_PaymentStatusModel>(e =>
            {
                e.HasKey(x => x.PaymentStatusId);
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(255).IsRequired();
                e.HasIndex(x => x.Status).IsUnique();
            });

            modelBuilder.Entity<OrdersModel>(e =>
            {
                e.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");

                e.HasOne(x => x.UserOrderLog)
                    .WithOne(x => x.Order)
                    .HasForeignKey<OrdersModel>(x => x.UserOrderLogID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.DeliveryLog)
                    .WithOne(x => x.Order)
                    .HasForeignKey<OrdersModel>(x => x.DeliveryLogID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.PaymentType)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(x => x.PaymentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.PaymentStatus)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(x => x.PaymentStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);

        }
    }
}
