using Newtonsoft.Json;

namespace InvestmentConsumer.Domain.ResponseObjects.DTOs
{
    public class InvestmentDto
    {
        public static InvestmentDto Desserialize(string jsonMessage)
        {
            try
            {
                return JsonConvert.DeserializeObject<InvestmentDto>(jsonMessage) ?? new InvestmentDto();
            }
            catch (Exception ex)
            {
                return new InvestmentDto();
            }
        }
        public int InvesmentId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
    }
}
