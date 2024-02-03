using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Persistance.Context
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=CustomerManagementDB;User Id=******;Password=*******;");
            optionsBuilder.UseNpgsql("Server=customer-management-database.postgres.database.azure.com;Database=customermanagementdatabase;Port=5432;User Id=******;Password=*******;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

