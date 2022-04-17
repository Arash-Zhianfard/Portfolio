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

            var currentAssetCount = positions.Where(x => x.TransactionType == TransactionType.Buy).Sum(x => x.Contract) -
                                    positions.Where(x => x.TransactionType == TransactionType.Sell).Sum(x => x.Contract);

            //the current profit should be zero because currently user has no asset
            if (currentAssetCount == 0)
                return 0;

            var buyCost = positions.Where(x => x.TransactionType == TransactionType.Buy).Sum(x => x.Price * x.Contract) -
                          positions.Where(x => x.TransactionType == TransactionType.Sell).Sum(x => x.Price * x.Contract);

            var breakevenPrice = buyCost / currentAssetCount;

            var totalProfit = (currentPrice - breakevenPrice) * currentAssetCount / buyCost * 100;

            return Math.Round(totalProfit, 2);
        }
    }
}