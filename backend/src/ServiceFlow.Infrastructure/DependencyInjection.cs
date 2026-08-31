using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceFlow.Infrastructure.Persistence;
using ServiceFlow.Application.Interfaces.Repositories;
using ServiceFlow.Infrastructure.Repositories;

namespace ServiceFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

            services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        
        return services;
    }
}