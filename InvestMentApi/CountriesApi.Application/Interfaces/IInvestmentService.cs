using CountriesApi.Application.Common;
using CountriesApi.Domain.ResponseObjects.DTOs;

namespace CountriesApi.Application.Interfaces
{
    public interface IInvestmentService
    {
        Task<Result<bool>> Invest(InvestmentDto investment);
    }
}
