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

        public async Task<ICollection<PortfolioItem>> Get(int protfolioId)
        {
            List<PortfolioItem> lst = new List<PortfolioItem>();
            var portfolio = await _portfolioRepository.GetPortfolioItems(protfolioId);
            var stockList = new List<string>();
            if (portfolio.Positions == null)
            {
                return lst;
            }
            foreach (var item in portfolio.Positions)
            {
                stockList.Add(item.Stock.Symbol);
            }
            var currentData = await _vwdService.GetAsync(stockList);
            var g = portfolio.Positions.GroupBy(x => x.Stock.Symbol);
            foreach (var item in g)
            {
                foreach (var onlineItem in currentData)
                {
                    if (item.Key == onlineItem.VwdKey)
                    {
                        lst.Add(new PortfolioItem
                        {

                            Symbol = item.FirstOrDefault().Stock.Symbol,
                            Price = double.Parse( onlineItem.Price.ToString("#.##")),
                            Name = item.FirstOrDefault().Stock.Name,
                            Bought = double.Parse(item.OrderByDescending(x => x.TransactionType).FirstOrDefault().Bought.ToString("#.##")),
                            Current = double.Parse((onlineItem.Price * item.Sum(x => x.Contract)).ToString("#.##")),
                            Quantity = double.Parse((item.Where(x => x.TransactionType == TransactionType.Buy).Sum(x => x.Contract) - item.Where(x => x.TransactionType == TransactionType.sell).Sum(x => x.Contract)).ToString("#.##")),
                            Yield = double.Parse(_profitCalculator.CalcTotalProfit(item.ToList(), onlineItem.Price).ToString("#.##"))
                        });
                        break;
                    }
                }
            }

            return lst;
        }

        public Task<ICollection<Portfolio>> GetByUserId(int userId)
        {
            return _portfolioRepository.GetByUserId(userId);
        }


    }
}
