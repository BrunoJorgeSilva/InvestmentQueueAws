using CountriesApi.Domain.ResponseObjects.DTOs;

namespace CountriesApi.Application.Interfaces
{
    public interface IInvesmentIntegration
    {
        Task<bool> Invest(InvestmentDto invesmentDto);
    }
}
