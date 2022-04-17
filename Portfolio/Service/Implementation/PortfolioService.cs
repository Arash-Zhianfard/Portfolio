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

        public async Task<ICollection<PortfolioItem>> Get(int portfolioId)
        {
            List<PortfolioItem> portfolioItems = new List<PortfolioItem>();
            var portfolio = await _portfolioRepository.GetPortfolioItems(portfolioId);
            var stockList = new List<string>();
            if (portfolio == null || !portfolio.Positions.Any())
            {
                return portfolioItems;
            }
            foreach (var item in portfolio.Positions.DistinctBy(x => x.Stock.Symbol))
            {
                stockList.Add(item.Stock.Symbol);
            }
            var currentData = await _vwdService.GetAsync(stockList);
            var groupedPositions = portfolio.Positions.GroupBy(x => x.Stock.Symbol);

            foreach (var item in groupedPositions)
            {
                foreach (var onlineItem in currentData)
                {
                    if (item.Key == onlineItem.VwdKey)
                    {
                        var currentAssetContract = GetCurrentAssetContract(item);
                        portfolioItems.Add(new PortfolioItem
                        {
                            PositionId = item.First().Id,
                            StockId = item.First().StockId,
                            Symbol = item.FirstOrDefault()?.Stock.Symbol ?? "",
                            Price = double.Parse(onlineItem.Price.ToString("#.##")),
                            Name = item.FirstOrDefault()?.Stock.Name ?? "",
                            Bought = double.Parse(item.OrderByDescending(x => x.CreateAt).First().Price.ToString("#.##")) * currentAssetContract,
                            Current = double.Parse((onlineItem.Price * currentAssetContract).ToString("#.##")),
                            Quantity = currentAssetContract,
                            Yield = double.Parse(_profitCalculator.CalcTotalProfit(item.ToList(), onlineItem.Price).ToString("#.##"))
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

        public Task<Portfolio> GetPortfolio(int id)
        {
            return _portfolioRepository.GetAsync(id);
        }
        public Task Delete(Portfolio portfolio)
        {
            return _portfolioRepository.DeleteAsync(portfolio);
        }

        public Task<ICollection<Portfolio>> GetByUserId(int userId)
        {
            return _portfolioRepository.GetByUserId(userId);
        }
    }
}
