using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{

    public class ProfitCalculator : IProfitCalculator
    {
        public double CalcTotalProfit(List<Position> positions, double currentPrice)
        {
            double totalProfit = 0;
            int currentAssetCount=0;
            var orderdPostion = positions.OrderByDescending(x => x.CreateAt);
            if (positions == null || !positions.Any()) 
                throw new ArgumentNullException(nameof(positions));
            foreach (var item in positions)
            {
                if (item.TransactionType == TransactionType.Buy) 
                {
                    totalProfit += (currentPrice - item.Price);
                    currentAssetCount += item.Contract;
                }
                else if (item.TransactionType == TransactionType.sell)
                {
                    currentAssetCount -= item.Contract;
                    if (currentAssetCount == 0) break;
                }
            }
            return 0;
        }
        public double CalcProfit(double boughtPrice, double currentPrice)
        {
            var yeild = ((currentPrice * 100) / boughtPrice) - 100;
            return yeild;
        }
    }
}
