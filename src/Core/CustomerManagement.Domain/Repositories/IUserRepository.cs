using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories.Common;

namespace CustomerManagement.Domain.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> IsEmailTakenAsync(string email);
        Task<bool> IsUsernameTakenAsync(string username);
    }
}

