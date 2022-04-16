namespace Abstraction.Models
{
    public class SellRequest
    {
        public string Symbol { get; set; }
        public int UserId { get; set; }
        public int Contract { get; set; }
        public double Price { get; set; }
        public int PortfolioId { get; set; }
        public string CurrencyName { get; set; }
    }
}
