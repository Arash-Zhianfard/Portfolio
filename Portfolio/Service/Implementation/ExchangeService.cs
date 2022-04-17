﻿using Abstraction.Interfaces.Services;
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
        public async Task<Position> RemovePosition(PositionRequest positionRequest)
        {
            if (positionRequest.Contract < 0 || positionRequest.Price < 0)
            {
                throw new CustomException("values Should be greater than zero");
            }
            Stock? stock = null;
            stock = await _stockService.GetAsync(positionRequest.Symbol, positionRequest.UserId);
            if (stock == null) throw new CustomException("No symbol found");
            if (stock.CurrentAssetContract < positionRequest.Contract)
            {
                throw new CustomException("number of sell should be equal or less than current asset");
            }
            var euro = await _currencyConvertor.Convert(positionRequest.CurrencyName, "EUR");
            var pricInEuro = GetPriceInEuro(positionRequest.Price, euro);
            var position = await _positionService.AddAsync(new Position()
            {
                PortfolioId = positionRequest.PortfolioId,
                StockId = stock.Id,
                Contract = positionRequest.Contract,
                Price = pricInEuro,
                Bought = pricInEuro * positionRequest.Contract,
                TransactionType = TransactionType.Sell
            });
            return position;
        }

        public double GetPriceInEuro(double price, double euroRate)
        {
            var priceInEuro = euroRate * price;
            return priceInEuro;
        }

        public async Task<Position> AddPosition(PositionRequest positionRequest)
        {
            Stock? stock = null;
            Stock? newStock = null;
            stock = await _stockService.GetAsync(positionRequest.Symbol, positionRequest.UserId);
            if (stock == null)
            {
                var onlineStockInfo = await _vwdService.GetAsync(positionRequest.Symbol);
                if (string.IsNullOrEmpty(onlineStockInfo.Name)) throw new CustomException("symbol not found");
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
            var euro = await _currencyConvertor.Convert(positionRequest.CurrencyName, "EUR");
            var priceInEuro = GetPriceInEuro(positionRequest.Price, euro);
            var addedStock = await _stockService.AddAsync(newStock);
            var position = await _positionService.AddAsync(new Position()
            {
                PortfolioId = positionRequest.PortfolioId,
                StockId = addedStock.Id,
                Contract = positionRequest.Contract,
                Price = priceInEuro,
                Bought = priceInEuro * positionRequest.Contract,
            });
            return position;

        }

    }
}

