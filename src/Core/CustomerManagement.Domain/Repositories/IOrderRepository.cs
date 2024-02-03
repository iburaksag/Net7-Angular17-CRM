using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories.Common;

namespace CustomerManagement.Domain.Repositories
{
	public interface IOrderRepository : IBaseRepository<Order>
	{
        Task<List<Order>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<List<Order>> GetOrdersByCustomerIdWithOrderAsync(Guid customerId);
    }
}

