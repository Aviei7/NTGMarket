using Application.DTO.Input.Checkout;
using Application.DTO.Output.Checkout;
using Application.Interfaces.Checkout;
using Domain.Models.StaticModel;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Models.UsersModel;
using Domain.Models.OrdersModels;

namespace Infrastructure.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly BoardStoreContext _context;

        public CheckoutRepository(BoardStoreContext context)
        {
            _context = context;
        }

        public async Task<List<s_DeliveryTypesModel>> GetDeliveryMethods()
        {
            return await _context.s_DeliveryTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<s_PaymentTypesModel>> GetPaymentMethods()
        {
            return await _context.s_PaymentTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<CheckoutSummaryDto> GetSummary()
        {
            throw new NotImplementedException();
        }

        public Task<CheckoutValidationResultDto> ValidateCheckout(CheckoutValidateDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<OrdersModel> SubmitCheckout(CheckoutSubmitDto dto, int userID)
        {
            var userOrderLogId = await CreateUserOrderLog(dto, userID);
            var deliveryLogId = await CreateDeliveryLogID(dto);
            var order = await CreateOrders(dto, userOrderLogId, deliveryLogId);

            return order;
        }

        private async Task<int> CreateUserOrderLog(CheckoutSubmitDto dto, int userID)
        {
            var userOrderLog = new UserOrderLogModel
            {
                FirstName = dto.FirstName,
                Lastname = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                UserID = userID
            };

            await _context.UserLogs.AddAsync(userOrderLog);
            await _context.SaveChangesAsync();

            return userOrderLog.UserOrderLogID;
        }

        private async Task<int> CreateDeliveryLogID(CheckoutSubmitDto dto)
        {
            var deliveryLogModel = new DeliveryLogModel
            {
                Address = dto.Address,
                City = dto.City,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                DeliveryMethod = dto.DeliveryMethod,
                SendDate = DateTime.UtcNow.AddDays(3)
            };

            await _context.DeliveryLogs.AddAsync(deliveryLogModel);
            await _context.SaveChangesAsync();

            return deliveryLogModel.DeliveryLogID;
        }

        private async Task<OrdersModel> CreateOrders(CheckoutSubmitDto dto, int userOrderLogId, int deliveryLogId)
        {
            var orderModel = new OrdersModel
            {
                UserOrderLogID = userOrderLogId,
                DeliveryLogID = deliveryLogId,
                PaymentTypeId = dto.PaymentMethod,
                PaymentStatusId = dto.PaymentStatusId,
                TotalPrice = dto.TotalPrice,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Orders.AddAsync(orderModel);
            await _context.SaveChangesAsync();

            return orderModel;
        }
    }
}
