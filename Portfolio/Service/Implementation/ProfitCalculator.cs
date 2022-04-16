using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{

    public class ProfitCalculator : IProfitCalculator
    {
        public double CalcTotalProfit(List<Position> positions, double currentPrice)
        {
            if (positions == null || !positions.Any()) 
                throw new ArgumentNullException(nameof(positions));
            var boughtPrice = positions.OrderByDescending(x => x.CreateAt).FirstOrDefault().Price;
            return CalcProfit(boughtPrice, currentPrice);
        }
        public double CalcProfit(double boughtPrice, double currentPrice)
        {
            var yeild = ((currentPrice * 100) / boughtPrice) - 100;
            return yeild;
        }
    }
}
