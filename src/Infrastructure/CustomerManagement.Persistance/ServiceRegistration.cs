using CustomerManagement.Application.Abstractions;
using CustomerManagement.Application.Customers.Commands.CreateCustomer;
using CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using CustomerManagement.Application.Users.Commands.Login;
using CustomerManagement.Application.Users.Commands.Register;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Domain.Repositories.Common;
using CustomerManagement.Persistance.Authentication;
using CustomerManagement.Persistance.Repositories;
using CustomerManagement.Persistance.Repositories.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Persistance
{
	public static class ServiceRegistration
	{
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Register services
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register validators
            services.AddTransient<IValidator<LoginCommand>, LoginCommandValidator>();
            services.AddTransient<IValidator<RegisterCommand>, RegisterCommandValidator>();
            services.AddTransient<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
            services.AddTransient<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>();

            return services;
        }
    }
}

