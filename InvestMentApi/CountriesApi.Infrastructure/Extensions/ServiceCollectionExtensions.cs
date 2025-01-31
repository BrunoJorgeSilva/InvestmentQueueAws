using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CountriesApi.Application.Interfaces;
using CountriesApi.Infrastructure.External;

namespace CountriesApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        { 
            services.AddScoped<IInvesmentIntegration, InvestmentIntegration>();
            return services;
        }
    }
}