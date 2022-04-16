namespace Abstraction.Models
{
    public class PositionRequest
    {
        public string Symbol { get; set; }
        public int PortfolioId { get; set; }
        public int UserId { get; set; }
        public int Contract { get; set; }
        public double Price { get; set; }
        public string CurrencyName { get; set; }
    }
}
