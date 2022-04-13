using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockService _stockService;
        private readonly IPositionService _positionService;
        private readonly IVwdService _vwdService;

        public PortfolioService(IPortfolioRepository portfolioService, IStockService stockService,
            IPositionService positionService, IVwdService vwdService)
        {
            _portfolioRepository = portfolioService;
            _stockService = stockService;
            _positionService = positionService;
            _vwdService = vwdService;
        }

        public Task<Portfolio> AddAsync(Portfolio entity)
        {
            return _portfolioRepository.AddAsync(entity);
        }



        public async Task<ICollection<PortfolioItem>> Get(int protfolioId)
        {
            List<PortfolioItem> lst = new List<PortfolioItem>();
            var portfolio = await _portfolioRepository.GetAsync(protfolioId);

            foreach (var item in portfolio.Positions)
            {
                var currentData = await _vwdService.GetAsync(item.Stock.Symbol);
                lst.Add(new PortfolioItem
                {
                    Symbol = item.Stock.Symbol,
                    Price = currentData.Price,
                    Name = item.Stock.Name,
                    Current = currentData.Price * item.Contract,
                    Bought = item.Bought,
                    Quantity = item.Contract,
                    Yield = CalcYeild(item.Bought, currentData.Price, item.Contract)
                });
            }
            return lst;
        }

        public double CalcYeild(double totalBoughtPrice, double ItemCurrentPrice, int quaintity)
        {
            var sumCurrentPrice = (ItemCurrentPrice * quaintity);
            var yeild = ((sumCurrentPrice * 100) / totalBoughtPrice) - 100;
            return Math.Round(yeild, 2);
        }

        public Task<ICollection<Portfolio>> GetByUserId(int userId)
        {
            return _portfolioRepository.GetByUserId(userId);
        }
    }
}
