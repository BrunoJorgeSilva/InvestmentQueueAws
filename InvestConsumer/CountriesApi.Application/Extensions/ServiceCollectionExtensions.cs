using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CountrInvestmentConsumeriesApi.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IInvestmentService, InvestmentService>();
            return services;
        }
    }
}