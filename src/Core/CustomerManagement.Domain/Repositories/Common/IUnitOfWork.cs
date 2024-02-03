namespace CustomerManagement.Domain.Repositories.Common
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}

