using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Service.Implementation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    internal class ExchangeServiceTest
    {
        
        private Mock<IStockService> stockService;
        private Mock<IPositionService> positionService;
        private Mock<IVwdService> wdService;
        private Mock<IProfitCalculator> profitCalculator;
        private Mock<ICurrencyConvertor> currencyConvertor;
        [SetUp]
        public void Setup()
        {
            currencyConvertor=new Mock<ICurrencyConvertor>();
            currencyConvertor.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(2);
            var position = new Position()
            {
                Bought = 100,
                Contract = 20,
                Id = 1,
                Stock = new Stock() { Id = 1, Isin = "123", Name = "StockName", Symbol = "Symbol1" },
                StockId = 0,
            };
            stockService = new Mock<IStockService>();
            positionService = new Mock<IPositionService>();
            stockService.Setup(x => x.GetAsync("StockName", It.IsAny<int>())).ReturnsAsync(new Stock() { Id = 1, Positions = new List<Position> { position } });
            positionService.Setup(x => x.GetAsync("StockName", It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(position)
                ;
            wdService = new Mock<IVwdService>();
            wdService.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new VwdResponse()
            {
                Name = "Stock1"
            });
            profitCalculator = new Mock<IProfitCalculator>();
            profitCalculator.Setup(x => x.CalcTotalProfit(It.IsAny<List<Position>>(), It.IsAny<double>())).Returns(1);

        }

        [Test]
        public void Sell_ShouldThrowExceptionIfSellMoreThanItHave()
        {
            var exchangeService = new ExchangeService(stockService.Object,
                positionService.Object, wdService.Object, currencyConvertor.Object);
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new SellRequest()
               { Contract = 21, Price = 10, Symbol = "StockName", UserId = 1 }));
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfSymbolNotFound()
        {
            var exchangeService = new ExchangeService(stockService.Object, positionService.Object, wdService.Object, currencyConvertor.Object);
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new SellRequest()
               { Contract = 1, Price = 10, Symbol = "UnKnownStockName", UserId = 1 }));
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfCotractNoGreaterthanZero()
        {
            var exchangeService = new ExchangeService(stockService.Object, positionService.Object, wdService.Object, currencyConvertor.Object);
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new SellRequest()
               { Contract = 0, Price = 10, Symbol = "Stack1", UserId = 1 }));
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfPriceNoGreaterthanZero()
        {
            var exchangeService = new ExchangeService(stockService.Object, positionService.Object, wdService.Object, currencyConvertor.Object);
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new SellRequest()
               { Contract = 10, Price = -10, Symbol = "Stack1", UserId = 1 }));
        }
    }
}
