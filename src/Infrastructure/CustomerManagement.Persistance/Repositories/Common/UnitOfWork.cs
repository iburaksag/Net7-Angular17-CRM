using CustomerManagement.Domain.Repositories.Common;
using CustomerManagement.Persistance.Context;

namespace CustomerManagement.Persistance.Repositories.Common
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}

