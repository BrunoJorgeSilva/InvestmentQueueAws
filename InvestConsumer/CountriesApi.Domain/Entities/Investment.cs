using InvestmentConsumer.Domain.ResponseObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentConsumer.Domain.Entities
{
    public class Investment
    {
        public Investment(InvestmentDto invesmentDto)
        {
            InvestmentId = invesmentDto.InvesmentId;
            ProductId = invesmentDto.ProductId;
            ClientId = invesmentDto.ClientId;
            Amount = invesmentDto.Amount;
            Invested = true ? 1 : 0;
        }
        public int InvestmentId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public int Invested { get; set; }
    }
}
