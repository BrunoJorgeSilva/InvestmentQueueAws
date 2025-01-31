using InvestmentConsumer.Application.Common;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;

namespace InvestmentConsumer.Application.Interfaces
{
    public interface IInvestmentService
    {
        Task<Result<bool>> Invest(InvestmentDto investment);
    }
}
