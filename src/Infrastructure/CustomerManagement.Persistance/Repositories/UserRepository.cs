using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Persistance.Context;
using CustomerManagement.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Persistance.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmailAsync(String email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByUsernameAsync(String username)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailTakenAsync(String email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsUsernameTakenAsync(String username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }
    }
}

