using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Persistance.Context;
using CustomerManagement.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Persistance.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Customer>> GetAllCustomersByCreatedDateAsync()
        {
            var customers = await GetAllAsync();
            var sortedOrders = customers.OrderByDescending(b => b.CreatedDate).ToList();
            return sortedOrders;
        }

        public async Task<bool> IsCustomerExistAsync(string email)
        {
            return await _dbContext.Customers.AnyAsync(u => u.Email == email);
        }
    }
}

