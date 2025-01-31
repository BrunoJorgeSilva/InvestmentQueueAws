using InvestmentConsumer.Domain.Entities;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;

namespace InvestmentConsumer.Application.Interfaces
{
    public interface IInvesmentRepository
    {
        Task<bool> Invest(Investment invesment);
    }
}
