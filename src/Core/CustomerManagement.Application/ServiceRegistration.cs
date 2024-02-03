using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Application
{
	public static class ServiceRegistration
	{
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(ServiceRegistration).Assembly;

            services.AddMediatR(configuration =>
                configuration.AsScoped(),
                assembly);

            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}

