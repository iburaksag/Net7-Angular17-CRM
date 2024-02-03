using Microsoft.EntityFrameworkCore;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Persistance.Context;
using CustomerManagement.Persistance.Repositories.Common;

namespace CustomerManagement.Persistance.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            return await _dbContext.Orders.Where(b => b.CustomerId == customerId).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByCustomerIdWithOrderAsync(Guid customerId)
        {
            var orders = await GetOrdersByCustomerIdAsync(customerId);
            var sortedOrders = orders.Where(b => b.Status == Status.Completed)
                                        .OrderByDescending(b => b.CreatedDate)
                                        .ToList();
            return sortedOrders;
        }
    }
}

