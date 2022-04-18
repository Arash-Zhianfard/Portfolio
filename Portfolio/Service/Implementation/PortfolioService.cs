using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IVwdService _vwdService;
        private readonly IProfitCalculator _profitCalculator;

        public PortfolioService(IPortfolioRepository portfolioService, IVwdService vwdService, IProfitCalculator profitCalculator)
        {
            _portfolioRepository = portfolioService;
            _vwdService = vwdService;
            _profitCalculator = profitCalculator;
        }

        public Task<Portfolio> AddAsync(Portfolio entity)
        {
            return _portfolioRepository.AddAsync(entity);
        }

        public async Task<ICollection<PortfolioItem>> GetItemsAsync(int portfolioId)
        {
            List<PortfolioItem> portfolioItems = new List<PortfolioItem>();
            var portfolio = await _portfolioRepository.GetPortfolioItemsAsync(portfolioId);
            var stockList = new List<string>();
            if (portfolio == null || !portfolio.Positions.Any())
            {
                return portfolioItems;
            }
            foreach (var item in portfolio.Positions.DistinctBy(x => x.Stock.Symbol))
            {
                stockList.Add(item.Stock.Symbol);
            }
            var onlineStockData = await _vwdService.GetAsync(stockList);
            var stockByPositions = portfolio.Positions.GroupBy(x => x.Stock.Symbol);

            foreach (var dbStockItem in stockByPositions)
            {
                foreach (var onlineStockItem in onlineStockData)
                {
                    if (dbStockItem.Key == onlineStockItem.VwdKey)
                    {
                        var currentAssetContract = GetCurrentAssetContract(dbStockItem);
                        var lastBuyPrice= dbStockItem.OrderByDescending(x => x.CreateAt).First().Price;
                        portfolioItems.Add(new PortfolioItem
                        {
                            PositionId = dbStockItem.First().Id,
                            StockId = dbStockItem.First().StockId,
                            Symbol = dbStockItem.FirstOrDefault()?.Stock.Symbol ?? "",
                            Price = double.Parse(onlineStockItem.Price.ToString("#.##")),
                            Name = dbStockItem.FirstOrDefault()?.Stock.Name ?? "",
                            Bought = double.Parse((lastBuyPrice * currentAssetContract).ToString("#.##")),
                            Current = double.Parse((onlineStockItem.Price * currentAssetContract).ToString("#.##")),
                            Quantity = currentAssetContract,
                            Yield = double.Parse(_profitCalculator.CalcTotalProfit(dbStockItem.ToList(), onlineStockItem.Price).ToString("#.##"))
                        });
                        break;
                    }
                }
            }
            return portfolioItems;
        }

        private int GetCurrentAssetContract(IGrouping<string, Position> item)
        {
            var currentAssetContract =
                item.Where(x => x.TransactionType == TransactionType.Buy).Sum(x => x.Contract) -
                item.Where(x => x.TransactionType == TransactionType.Sell).Sum(x => x.Contract);
            return currentAssetContract;
        }

        public Task<Portfolio> GetPortfolioAsync(int id)
        {
            return _portfolioRepository.GetAsync(id);
        }
        public Task DeleteAsync(Portfolio portfolio)
        {
            return _portfolioRepository.DeleteAsync(portfolio);
        }

        public Task<ICollection<Portfolio>> GetByUserIdAsync(int userId)
        {
            return _portfolioRepository.GetByUserIdAsync(userId);
        }
    }
}
