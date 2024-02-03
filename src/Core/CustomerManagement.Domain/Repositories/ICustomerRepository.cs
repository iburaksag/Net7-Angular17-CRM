using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories.Common;

namespace CustomerManagement.Domain.Repositories
{
	public interface ICustomerRepository : IBaseRepository<Customer>
	{
        Task<bool> IsCustomerExistAsync(string email);
        Task<List<Customer>> GetAllCustomersByCreatedDateAsync();
    }
}

