namespace CountriesApi.Domain.ResponseObjects.DTOs
{
    public class InvestmentDto
    {
        public int InvesmentId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
    }
}
