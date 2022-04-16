using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IProfitCalculator
    {
        public double CalcTotalProfit(List<Position> positions,double currentPrice);
    }
}
