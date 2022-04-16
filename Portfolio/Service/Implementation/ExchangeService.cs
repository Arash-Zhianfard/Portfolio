using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class ExchangeService : IExchangeService
    {

        private readonly IStockService _stockService;
        private readonly IPositionService _positionService;
        private readonly IVwdService _vwdService;
        private readonly ICurrencyConvertor _currencyConvertor;
        public ExchangeService(IStockService stockService,
            IPositionService positionService, IVwdService vwdService, ICurrencyConvertor currencyConvertor)
        {
            _stockService = stockService;
            _positionService = positionService;
            _vwdService = vwdService;
            _currencyConvertor = currencyConvertor;
        }
        public async Task<Position> RemovePosition(SellRequest sellRequest)
        {
            if (sellRequest.Contract < 0 || sellRequest.Price < 0)
            {
                throw new CustomException("values Should be greater than zero");
            }
            Stock? stock = null;
            stock = await _stockService.GetAsync(sellRequest.Symbol, sellRequest.UserId);
            if (stock == null) throw new CustomException("No symbol found");
            if (stock.Positions.Sum(x => x.Contract) < sellRequest.Contract)
            {
                throw new CustomException("number of sell should be equal or less than current asset");
            }
            var euro = await _currencyConvertor.Convert(sellRequest.CurrencyName, "EUR");
            var pricInEuro = euro * sellRequest.Price;
            var postition = await _positionService.AddAsync(new Position()
            {
                PortfolioId = sellRequest.PortfolioId,
                StockId = stock.Id,
                Contract = sellRequest.Contract,
                Price = pricInEuro,
                Bought = pricInEuro * sellRequest.Contract,
                TransactionType = TransactionType.sell
            });
            return postition;
        }

        public async Task<Position> AddPosition(PositionRequest posistionReqeust)
        {
            Stock? stock = null;
            Stock? newStock = null;
            stock = await _stockService.GetAsync(posistionReqeust.Symbol, posistionReqeust.UserId);
            if (stock == null)
            {
                var onlineStockInfo = await _vwdService.GetAsync(posistionReqeust.Symbol);
                if (string.IsNullOrEmpty(onlineStockInfo.Name)) throw new Exception("symbol not found");
                newStock = new Stock()
                {
                    Name = onlineStockInfo.Name,
                    Isin = onlineStockInfo.Isin,
                    Symbol = onlineStockInfo.VwdKey
                };
            }
            else
            {
                newStock = new Stock()
                {
                    Name = stock.Name,
                    Isin = stock.Isin,
                    Symbol = stock.Symbol
                };
            }
            var euro = await _currencyConvertor.Convert(posistionReqeust.CurrencyName, "EUR");
            var pricInEuro = euro * posistionReqeust.Price;
            var addedStock = await _stockService.AddAsync(newStock);
            var postition = await _positionService.AddAsync(new Position()
            {
                PortfolioId = posistionReqeust.PortfolioId,
                StockId = addedStock.Id,
                Contract = posistionReqeust.Contract,
                Price = pricInEuro,
                Bought = pricInEuro * posistionReqeust.Contract,
            });
            return postition;

        }

    }
}

