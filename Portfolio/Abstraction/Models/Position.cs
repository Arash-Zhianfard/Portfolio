namespace Abstraction.Models
{
    public class Position:BaseModel
    {
        public int Quaintity { get; set; }
        public int StockId { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public Stock Stock { get; set; }
    }   
}
