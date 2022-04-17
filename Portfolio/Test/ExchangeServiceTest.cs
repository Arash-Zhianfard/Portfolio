using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Moq;
using NUnit.Framework;
using Service.Implementation;
using System.Collections.Generic;

namespace Test
{
    internal class ExchangeServiceTest
    {

        private Mock<IStockService>? _stockService;
        private Mock<IPositionService>? _positionService;
        private Mock<IVwdService>? _wdService;
        private Mock<IProfitCalculator>? _profitCalculator;
        private Mock<ICurrencyConvertor>? _currencyConvertor;
        [SetUp]
        public void Setup()
        {
            _currencyConvertor = new Mock<ICurrencyConvertor>();
            _currencyConvertor.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(2);
            var position = new Position()
            {
                Bought = 100,
                Contract = 20,
                Id = 1,
                Stock = new Stock() { Id = 1, Isin = "123", Name = "StockName", Symbol = "Symbol1" },
                StockId = 0,
            };
            _stockService = new Mock<IStockService>();
            _positionService = new Mock<IPositionService>();
            _stockService.Setup(x => x.GetAsync("StockName", It.IsAny<int>(),It.IsAny<int>())).ReturnsAsync(new Stock() { Id = 1, Positions = new List<Position> { position } });
            _positionService.Setup(x => x.GetAsync("StockName", It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(position);
            _wdService = new Mock<IVwdService>();
            _wdService.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new VwdResponse()
            {
                Name = "Stock1"
            });
            _profitCalculator = new Mock<IProfitCalculator>();
            _profitCalculator.Setup(x => x.CalcTotalProfit(It.IsAny<List<Position>>(), It.IsAny<double>())).Returns(1);

        }

        [Test]
        public void Sell_ShouldThrowExceptionIfSellMoreThanItHave()
        {
            //arrange
            var exchangeService = new ExchangeService(_stockService.Object,
                _positionService.Object, _wdService.Object, _currencyConvertor.Object);
            //act and assert
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new PositionRequest()
                   { Contract = 21, Price = 10, Symbol = "StockName", UserId = 1 }));
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfSymbolNotFound()
        {
            //arrange
            var exchangeService = new ExchangeService(_stockService.Object, _positionService.Object, _wdService.Object, _currencyConvertor.Object);
            //act and assert
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new PositionRequest()
                   { Contract = 1, Price = 10, Symbol = "UnKnownStockName", UserId = 1 }));
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfContractNotGreaterThanZero()
        {
            //arrange
            var exchangeService = new ExchangeService(_stockService.Object, _positionService.Object, _wdService.Object, _currencyConvertor.Object);
            //act and assert
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new PositionRequest()
                   { Contract = 0, Price = 10, Symbol = "Stack1", UserId = 1 }));
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfPriceNotGreaterThanZero()
        {
            //arrange
            var exchangeService = new ExchangeService(_stockService.Object, _positionService.Object, _wdService.Object, _currencyConvertor.Object);
            //act and assert
            Assert.ThrowsAsync<CustomException>(() => exchangeService.RemovePosition(
               new PositionRequest()
                   { Contract = 10, Price = -10, Symbol = "Stack1", UserId = 1 }));
        }
        [Test]
        public void Add_ShouldThrowExceptionIfSymbolNotFound()
        {
            //arrange
            _wdService.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new VwdResponse()
            {
            });
            var exchangeService = new ExchangeService(_stockService.Object, _positionService.Object, _wdService.Object, _currencyConvertor.Object);
            //act and assert
            Assert.ThrowsAsync<CustomException>(() => exchangeService.AddPosition(
                new PositionRequest()
                    { Contract = 1, Price = 10, Symbol = "UnKnownStockName", UserId = 1 }));
        }
        [Test]
        public void GetPriceInEuro()
        {
            //arrange
            const int expect = 8;
            var exchangeService = new ExchangeService(_stockService.Object, _positionService.Object, _wdService.Object, _currencyConvertor.Object);
            //act
            var result=exchangeService.GetPriceInEuro(10, 0.8);
            //assert
            Assert.AreEqual(expect, result);
        }
    }
}
