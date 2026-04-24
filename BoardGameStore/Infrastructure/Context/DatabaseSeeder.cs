using Application.Interfaces.DBContext;
using Domain.Models.FiltersModel;
using Domain.Models.LoyaltyModel;
using Domain.Models.ProductsModel;
using Domain.Models.StaticModel;
using Domain.Models.UserModel;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private const string GuestUserEmail = "guestorder@guest.com";
        private const string GuestUserFirstName = "Guest";
        private const string GuestUserLastName = "System";
        private const string GuestUserPhoneNumber = "+000000000000";

        readonly private BoardStoreContext _context;

        public DatabaseSeeder(BoardStoreContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            EnsureSystemUsers();
            ClearSeeds();

            using var transaction = _context.Database.BeginTransaction();
            SeedStaticGroup();
            MainGroup();
            transaction.Commit();

            SeedProductGroup();
            SeedFilterGroup();
        }

        public void EnsureSystemUsers()
        {
            EnsureGuestUser();
        }

        private void EnsureGuestUser()
        {
            if (_context.Users.Any(x => x.Email == GuestUserEmail))
            {
                return;
            }

            var guestUser = UsersModel.Create(
                GuestUserFirstName,
                GuestUserLastName,
                BCrypt.Net.BCrypt.HashPassword($"system-user:{GuestUserEmail}"),
                GuestUserEmail,
                GuestUserPhoneNumber);

            _context.Users.Add(guestUser);
            _context.SaveChanges();
        }

        private void SeedProductGroup()
        {
            SeedLaptops();
            SeedSmartWatches();
            SeedFoods();
            _context.SaveChanges();
            SeedProductImages();
        }

        private void SeedStaticGroup()
        {
            SeedTables();
            SeedOperation();
            SeedFilterTypes();
            SeedDeliveryTypes();
            SeedPaymentTypes();
            SeedPaymentStatuses();
            SeedDiscount();
        }

        private void SeedFilterGroup()
        {
            SeedFiltersOperations();
        }

        private void MainGroup()
        {
            /*Без категорий не вставяться товары*/
            SeedCategory();
            SeedFilters();
        }

        private void ClearSeeds()
        {
            if (_context.Orders.Any()) _context.Orders.RemoveRange(_context.Orders);
            if (_context.DeliveryLogs.Any()) _context.DeliveryLogs.RemoveRange(_context.DeliveryLogs);
            if (_context.UserLogs.Any()) _context.UserLogs.RemoveRange(_context.UserLogs);
            if (_context.ProductImages.Any()) _context.ProductImages.RemoveRange(_context.ProductImages);
            if (_context.Reviews.Any()) _context.Reviews.RemoveRange(_context.Reviews);
            if (_context.f_FilterOperations.Any()) _context.f_FilterOperations.RemoveRange(_context.f_FilterOperations);
            if (_context.f_Filters.Any()) _context.f_Filters.RemoveRange(_context.f_Filters);
            if (_context.Products.Any()) _context.Products.RemoveRange(_context.Products);
            if (_context.f_Categories.Any()) _context.f_Categories.RemoveRange(_context.f_Categories);
            if (_context.l_Dsicounts.Any()) _context.l_Dsicounts.RemoveRange(_context.l_Dsicounts);
            if (_context.s_Operations.Any()) _context.s_Operations.RemoveRange(_context.s_Operations);
            if (_context.s_FilterTypes.Any()) _context.s_FilterTypes.RemoveRange(_context.s_FilterTypes);
            if (_context.s_Tables.Any()) _context.s_Tables.RemoveRange(_context.s_Tables);
            if (_context.s_DeliveryTypes.Any()) _context.s_DeliveryTypes.RemoveRange(_context.s_DeliveryTypes);
            if (_context.s_PaymentTypes.Any()) _context.s_PaymentTypes.RemoveRange(_context.s_PaymentTypes);
            if (_context.s_PaymentStatus.Any()) _context.s_PaymentStatus.RemoveRange(_context.s_PaymentStatus);

            _context.SaveChanges();


            // Сброс identity
            ResetIdentity("Orders", "DeliveryLogs", "UserLogs", "Reviews", "ProductImages", "Products", "f_FilterOperations", "f_Filters", "f_Categories", "s_Operations", "s_FilterTypes", "s_Tables", "l_Dsicounts", "s_PaymentTypes", "s_PaymentStatus", "s_DeliveryTypes");

        }

        private void ResetIdentity(params string[] tableNames)
        {
            if (!_context.Database.IsSqlServer())
            {
                return;
            }

            foreach (var name in tableNames)
            {
                _context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('{name}', RESEED, 0)");
            }
        }

        private void SeedProductImages()
        {
            var productImages = new List<ProductImageModel>();

            void AddImagesForCategory(string categorySlug, string fileName)
            {
                var categoryId = GetRequiredCategoryId(categorySlug);
                var productIds = _context.Products
                    .Where(p => p.CategoryID == categoryId)
                    .Select(p => p.ID)
                    .ToList();

                foreach (var productId in productIds)
                {
                    productImages.Add(new ProductImageModel
                    {
                        ProductId = productId,
                        FileName = fileName,
                        IsPrimary = true
                    });
                }
            }

            AddImagesForCategory("laptop", "test_lap1.jpg");
            AddImagesForCategory("smart-watch", "test_smart_watch1.jpg");
            AddImagesForCategory("food", "test_leys_crab1.png");

            _context.ProductImages.AddRange(productImages);
            _context.SaveChanges();
        }

        private void SeedLaptops()
        {
            var random = new Random();
            var products = new List<ProductModel>();
            var categoryId = GetRequiredCategoryId("laptop");

            for (int i = 1; i <= 15; i++)
            {
                products.Add(new ProductModel
                {
                    Name = $"Laptop {i}",
                    Slug = $"laptop-{i}",
                    Stock = 1000,
                    CategoryID = categoryId,
                    Brand = "Lenovo",
                    Description = "Цей Пк є надзвичайно надійним і інноваційним для використання.",
                    Price = random.Next(3000, 30001),
                    IsActive = true
                });
            }

            _context.Products.AddRange(products);


        }

        private void SeedSmartWatches()
        {

            var random = new Random();
            var products = new List<ProductModel>();
            var categoryId = GetRequiredCategoryId("smart-watch");

            for (int i = 16; i <= 30; i++)
            {
                products.Add(new ProductModel
                {
                    Name = $"Xiaomi Smart Watches {i}",
                    Slug = $"Xiaomi-Smart-Watches-{i}",
                    Stock = 1000,
                    CategoryID = categoryId,
                    Description = "Найкращі часи з усього світу, з Китаю.",
                    Brand = "Xiaomi",
                    Price = random.Next(1000, 7000),
                    IsActive = true
                });
            }

            _context.Products.AddRange(products);


        }

        private void SeedFoods()
        {

            var random = new Random();
            var products = new List<ProductModel>();
            var categoryId = GetRequiredCategoryId("food");

            for (int i = 31; i <= 45; i++)
            {
                products.Add(new ProductModel
                {
                    Name = $"Leys with crab {i}",
                    Slug = $"Leys-with-crab-{i}",
                    Stock = 1000,
                    CategoryID = categoryId,
                    Description = "Леееееееееейсссс с крабом или ...",
                    Brand = "Leys",
                    Price = random.Next(10, 150),
                    IsActive = true
                });
            }

            _context.Products.AddRange(products);


        }

        /*Loyalty*/

        private void SeedDiscount()
        {

           var seed = new List<DiscountModel>
            {
                new DiscountModel { Name = "Новий рік", DiscountPercent = 15, IsActive = true },
                new DiscountModel { Name = "Подарунок", DiscountPercent = 30, PromoCode = "11", IsActive = true },
                new DiscountModel { Name = "Чорна п'ятниця", DiscountPercent = 20, IsActive = true }
            };


            _context.l_Dsicounts.AddRange(seed);
            _context.SaveChanges();
        }

        /*Filters*/

        private void SeedFilters()
        {
            var productsTableId = GetRequiredTableId("Products");
            var rangeSliderFilterTypeId = GetRequiredFilterTypeId("slider");
            var checkboxFilterTypeId = GetRequiredFilterTypeId("checkbox");

            var seed = new List<f_FilterInfoModel>
            {
                new f_FilterInfoModel { TableID = productsTableId, FilterName = "Ціна", FieldName = "Price", FilterTypeID = rangeSliderFilterTypeId },
                new f_FilterInfoModel { TableID = productsTableId, FilterName = "Бренд", FieldName = "Brand", FilterTypeID = checkboxFilterTypeId },
                new f_FilterInfoModel { TableID = productsTableId, FilterName = "Вага", FieldName = "Weight", FilterTypeID = checkboxFilterTypeId },
            };

            _context.f_Filters.AddRange(seed);
            _context.SaveChanges();

        }

        private void SeedFiltersOperations()
        {
            var priceFilterId = GetRequiredFilterId("Ціна");
            var brandFilterId = GetRequiredFilterId("Бренд");
            var weightFilterId = GetRequiredFilterId("Вага");
            var betweenOperationId = GetRequiredOperationId("between");
            var inOperationId = GetRequiredOperationId("In");
            var lessThanOperationId = GetRequiredOperationId("lessThan");

            var seed = new List<f_FilterOperationModel>
            {
                 new f_FilterOperationModel { FilterID = priceFilterId, OperationID = betweenOperationId, OperationName = "Ціна", QueryParam = "betweenPrice" },
                 /*Brand*/
                 new f_FilterOperationModel { FilterID = brandFilterId, OperationID = inOperationId, OperationName = "Acer", QueryParam = "inAcer" },
                 new f_FilterOperationModel { FilterID = brandFilterId, OperationID = inOperationId, OperationName = "Lenovo", QueryParam = "inLenovo" },
                 new f_FilterOperationModel { FilterID = brandFilterId, OperationID = inOperationId, OperationName = "Legion", QueryParam = "inLegion" },
                 new f_FilterOperationModel { FilterID = brandFilterId, OperationID = inOperationId, OperationName = "Xiaomi", QueryParam = "inXiaomi" },
                 new f_FilterOperationModel { FilterID = brandFilterId, OperationID = inOperationId, OperationName = "Leys", QueryParam = "inLeys" },
                 /*Weight*/
                 new f_FilterOperationModel { FilterID = weightFilterId, OperationID = lessThanOperationId, OperationName = "До 1 кг", QueryParam = "lessThanWeight" },
                 new f_FilterOperationModel { FilterID = weightFilterId, OperationID = betweenOperationId, OperationName = "1 - 5 кг", QueryParam = "betweenWeight" },
                 new f_FilterOperationModel { FilterID = weightFilterId, OperationID = betweenOperationId, OperationName = "5 - 10 кг", QueryParam = "betweenWeight" },
                 new f_FilterOperationModel { FilterID = weightFilterId, OperationID = betweenOperationId, OperationName = "10 - 30 кг", QueryParam = "betweenWeight" }
            };

            _context.f_FilterOperations.AddRange(seed);
            _context.SaveChanges();

        }

        private void SeedCategory()
        {
            var seed = new List<f_CategoryModel>
            {
                 new f_CategoryModel { CategoryName = "Laptops", CategorySlug = "laptop", IsActive = true },
                 new f_CategoryModel { CategoryName = "Smart Watches", CategorySlug = "smart-watch", IsActive = true },
                 new f_CategoryModel { CategoryName = "Foods", CategorySlug = "food", IsActive = true }
            };

            _context.f_Categories.AddRange(seed);
            _context.SaveChanges();
        }

        /*Static*/
        private void SeedFilterTypes()
        {
            var seed = new List<s_FilterTypeModel>
            {
                new s_FilterTypeModel { ComponentName = "TextBox", ComponentHint = "input", HasRange = false, HasMultiple = false },
                new s_FilterTypeModel { ComponentName = "CheckBox", ComponentHint = "checkbox", HasRange = false, HasMultiple = true },
                new s_FilterTypeModel { ComponentName = "RangeSlider", ComponentHint = "slider", HasRange = true, HasMultiple = false },
                new s_FilterTypeModel { ComponentName = "DropDown", ComponentHint = "select", HasRange = false, HasMultiple = true },
                new s_FilterTypeModel { ComponentName = "DateRange", ComponentHint = "date-range", HasRange = true, HasMultiple = false }
            };

            _context.s_FilterTypes.AddRange(seed);
            _context.SaveChanges();

        }

        private void SeedDeliveryTypes()
        {
            var seed = new List<s_DeliveryTypesModel>
            {
                new s_DeliveryTypesModel { DeliveryName = "Нова пошта", Description = "Доставка у відділення або поштомат." },
                new s_DeliveryTypesModel { DeliveryName = "Кур'єр", Description = "Адресна доставка кур'єром по місту." },
                new s_DeliveryTypesModel { DeliveryName = "Самовивіз", Description = "Самостійне отримання замовлення з магазину." }
            };

            _context.s_DeliveryTypes.AddRange(seed);
            _context.SaveChanges();
        }

        private void SeedPaymentTypes()
        {
            var seed = new List<s_PaymentTypesModel>
            {
                new s_PaymentTypesModel { PaymentName = "Оплата карткою", Description = "Оплата банківською карткою під час оформлення." },
                new s_PaymentTypesModel { PaymentName = "Готівкою при отриманні", Description = "Оплата готівкою під час отримання замовлення." },
                new s_PaymentTypesModel { PaymentName = "Онлайн-оплата", Description = "Миттєва оплата через платіжний сервіс." }
            };

            _context.s_PaymentTypes.AddRange(seed);
            _context.SaveChanges();
        }

        private void SeedPaymentStatuses()
        {
            var seed = new List<s_PaymentStatusModel>
            {
                new s_PaymentStatusModel { Name = "Очікує оплату", Description = "Платіж створено, але ще не підтверджено.", Status = 1 },
                new s_PaymentStatusModel { Name = "Оплачено", Description = "Платіж успішно проведено та підтверджено.", Status = 2 },
                new s_PaymentStatusModel { Name = "Помилка оплати", Description = "Платіж не був проведений через помилку.", Status = 3 },
                new s_PaymentStatusModel { Name = "Повернено", Description = "Кошти були повернуті покупцю.", Status = 4 },
                new s_PaymentStatusModel { Name = "Скасовано", Description = "Платіж або замовлення було скасовано.", Status = 5 }
            };

            _context.s_PaymentStatus.AddRange(seed);
            _context.SaveChanges();
        }


        private void SeedTables()
        {
            var seed = new List<s_TablesModel>
            {
                new s_TablesModel { TableName = "Products", Description = "Каталог товарів магазину." },
                new s_TablesModel { TableName = "ProductImages", Description = "Зображення товарів та позначка головного фото." },
                new s_TablesModel { TableName = "Reviews", Description = "Відгуки користувачів про товари." },
                new s_TablesModel { TableName = "f_Categories", Description = "Категорії товарів каталогу." },
                new s_TablesModel { TableName = "f_Filters", Description = "Налаштування фільтрів для таблиць та сторінок." },
                new s_TablesModel { TableName = "f_FilterOperations", Description = "Доступні операції та параметри для фільтрів." },
                new s_TablesModel { TableName = "s_Tables", Description = "Довідник таблиць системи та їх описів." },
                new s_TablesModel { TableName = "s_Operations", Description = "Довідник операцій для побудови фільтрів." },
                new s_TablesModel { TableName = "s_FilterTypes", Description = "Довідник типів UI-компонентів для фільтрів." },
                new s_TablesModel { TableName = "s_DeliveryTypes", Description = "Довідник способів доставки замовлень." },
                new s_TablesModel { TableName = "s_PaymentTypes", Description = "Довідник способів оплати замовлень." },
                new s_TablesModel { TableName = "s_PaymentStatus", Description = "Довідник статусів оплати замовлень." },
                new s_TablesModel { TableName = "Users", Description = "Облікові записи користувачів системи." },
                new s_TablesModel { TableName = "l_Dsicounts", Description = "Знижки, промокоди та програми лояльності." },
                new s_TablesModel { TableName = "Orders", Description = "Замовлення користувачів." },
                new s_TablesModel { TableName = "UserLogs", Description = "Зафіксовані контактні дані користувача на момент оформлення замовлення." },
                new s_TablesModel { TableName = "DeliveryLogs", Description = "Зафіксовані дані доставки на момент оформлення замовлення." }
            };

            _context.s_Tables.AddRange(seed);
            _context.SaveChanges();

        }

        private int GetRequiredCategoryId(string categorySlug)
        {
            return _context.f_Categories
                .Where(x => x.CategorySlug == categorySlug)
                .Select(x => x.CategoryID)
                .Single();
        }

        private int GetRequiredTableId(string tableName)
        {
            return _context.s_Tables
                .Where(x => x.TableName == tableName)
                .Select(x => x.TableID)
                .Single();
        }

        private int GetRequiredFilterTypeId(string componentHint)
        {
            return _context.s_FilterTypes
                .Where(x => x.ComponentHint == componentHint)
                .Select(x => x.FilterTypeID)
                .Single();
        }

        private int GetRequiredFilterId(string filterName)
        {
            return _context.f_Filters
                .Where(x => x.FilterName == filterName)
                .Select(x => x.FilterID)
                .Single();
        }

        private int GetRequiredOperationId(string operationName)
        {
            return _context.s_Operations
                .Where(x => x.OperationName == operationName)
                .Select(x => x.OperationID)
                .Single();
        }

        private void SeedOperation()
        {
            var seed = new List<s_OperationModel>
            {
                new s_OperationModel { OperationName = "Equal", ExpressionTemplate = "{field} == @0", OperationDescription = "Дорівнює" },
                new s_OperationModel { OperationName = "notEqual", ExpressionTemplate = "{field} != @0", OperationDescription = "Не дорівнює" },
                new s_OperationModel { OperationName = "greaterThan", ExpressionTemplate = "{field} > @0", OperationDescription = "Більше ніж" },
                new s_OperationModel { OperationName = "greaterOrEqual", ExpressionTemplate = "{field} >= @0", OperationDescription = "Більше або дорівнює" },
                new s_OperationModel { OperationName = "lessThan", ExpressionTemplate = "{field} < @0", OperationDescription = "Менше ніж" },
                new s_OperationModel { OperationName = "lessOrEqual", ExpressionTemplate = "{field} <= @0", OperationDescription = "Менше або дорівнює" },
                new s_OperationModel { OperationName = "between", ExpressionTemplate = "{field} >= @0 && {field} <= @1", OperationDescription = "Між" },
                new s_OperationModel { OperationName = "notContains", ExpressionTemplate = "!{field}.Contains(@0)", OperationDescription = "Не містить" },
                new s_OperationModel { OperationName = "startsWith", ExpressionTemplate = "{field}.StartsWith(@0)", OperationDescription = "Починається з" },
                new s_OperationModel { OperationName = "endsWith", ExpressionTemplate = "{field}.EndsWith(@0)", OperationDescription = "Закінчується на" },
                new s_OperationModel { OperationName = "isNull", ExpressionTemplate = "{field} == null", OperationDescription = "Порожнє значення" },
                new s_OperationModel { OperationName = "In", ExpressionTemplate = "{field}.Contains(@0)", OperationDescription = "Входить у список" },
                new s_OperationModel { OperationName = "notIn", ExpressionTemplate = "!{field}.Contains(@0)", OperationDescription = "Не входить у список" }
            };

            _context.s_Operations.AddRange(seed);
            _context.SaveChanges();

        }
    }
}
