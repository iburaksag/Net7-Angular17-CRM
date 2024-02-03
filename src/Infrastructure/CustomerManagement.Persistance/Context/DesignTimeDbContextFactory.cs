using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CustomerManagement.Persistance.Context
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            //String connectionString = "Server=localhost;Port=5432;Database=CustomerManagementDB;User Id=postgres;Password=Iavenjq97*;";
            String connectionString = "Server=customer-management-database.postgres.database.azure.com;Database=customermanagementdatabase;Port=5432;User Id=******;Password=*******;";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseNpgsql(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}

