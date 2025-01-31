using InvestmentConsumer.Domain.ResponseObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentConsumer.Application.Interfaces
{
    public interface IQueueService
    {
        Task<List<InvestmentDto>> ReceiveMessagesAsync();
    }
}
