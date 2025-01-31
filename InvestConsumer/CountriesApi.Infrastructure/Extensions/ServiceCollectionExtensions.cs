using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using InvestmentConsumer.Infrastructure.External;
using InvestmentConsumer.Application.Interfaces;

namespace InvestmentConsumer.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        { 
            services.AddScoped<IInvesmentRepository, InvestmentRepository>();
            return services;
        }
    }
}