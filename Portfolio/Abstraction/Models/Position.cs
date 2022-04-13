namespace Abstraction.Models
{
    public class Position:BaseModel
    {
        public int Contract { get; set; }
        public int StockId { get; set; }
        public int PortfolioId { get; set; }
        public double Bought { get; set; }
        public Portfolio Portfolio { get; set; }
        public Stock Stock { get; set; }
    }   
}
