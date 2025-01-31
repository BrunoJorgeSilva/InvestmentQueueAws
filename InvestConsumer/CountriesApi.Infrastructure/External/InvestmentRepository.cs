using Dapper;
using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Domain.Entities;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace InvestmentConsumer.Infrastructure.External
{
    public class InvestmentRepository : IInvesmentRepository
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("OracleInvestmentConnection") ?? string.Empty; // Adicione sua connection string aqui
        private readonly ILogger<InvestmentRepository> _logger;

        public InvestmentRepository(string connectionString, ILogger<InvestmentRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<bool> Invest(Investment investment)
        {
            try
            {
                const string query = @"
                    INSERT INTO InvestmentTry (InvestmentId, ProductId, ClientId, Amount, Invested)
                    VALUES (:InvestmentId, :ProductId, :ClientId, :Amount, :Invested)";

                using (var connection = new OracleConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var rowsAffected = await connection.ExecuteAsync(query, new
                    {
                        investment.InvestmentId, 
                        investment.ProductId,
                        investment.ClientId,
                        investment.Amount,
                        investment.Invested
                    });
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[InvestmentRepository.Invest] Error inserting investment: {ex.Message}", ex);
                throw; 
            }
        }
    }
}