using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class ExchangeService : IExchangeService
    {

        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockService _stockService;
        private readonly IPositionService _positionService;
        private readonly IVwdService _vwdService;

        public ExchangeService(IPortfolioRepository portfolioService, IStockService stockService,
            IPositionService positionService, IVwdService vwdService)
        {
            _portfolioRepository = portfolioService;
            _stockService = stockService;
            _positionService = positionService;
            _vwdService = vwdService;
        }

        public Task Buy(BuyRequest buyRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Position> Sell(SellRequest sellRequest)
        {
            if (sellRequest.Contract < 0 || sellRequest.Price < 0)
            {
                throw new ArgumentException();
            }
            var stock = await _positionService.GetAsync(sellRequest.Symbol, sellRequest.UserId);
            if (stock == null) throw new Exception();
            if (stock.Contract < sellRequest.Contract)
            {
                throw new Exception();
            }
            else
            {
                stock.Contract -= sellRequest.Contract;
            }
            var result = await _positionService.UpdateAsync(stock);
            return result;
        }

        public async Task<Position> AddPostion(PositionRequest posistionReqeust)
        {
            var stockInfo = await _vwdService.GetAsync(posistionReqeust.Symbol);
            if (stockInfo != null)
            {
                Stock? stock = null;
                stock = await _stockService.GetAsync(posistionReqeust.Symbol, posistionReqeust.UserId);
                if (stock == null)
                {
                    stock = await _stockService.AddAsync(new Stock()
                    {
                        Name = stockInfo.Name
                    });
                    var postition = await _positionService.AddAsync(new Position()
                    {
                        PortfolioId = posistionReqeust.PortfolioId,
                        StockId = stock.Id,
                        Contract = posistionReqeust.Contract,
                        Bought = posistionReqeust.BuyPrice
                    });
                    return postition;
                }
                var oldPosition = stock.Positions.FirstOrDefault();
                var consolidatedPosition = ConsolidatePosition(posistionReqeust, oldPosition);
                await _positionService.UpdateAsync(consolidatedPosition);
                return oldPosition;
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }

        public Position ConsolidatePosition(PositionRequest newPosition, Position? oldPosition)
        {
            oldPosition.Contract += newPosition.Contract;
            oldPosition.Bought += newPosition.BuyPrice;
            return oldPosition;
        }
    }
}
