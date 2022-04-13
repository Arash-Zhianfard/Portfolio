using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Moq;
using NUnit.Framework;
using Service.Implementation;
using System;
using System.Collections.Generic;

namespace Test
{
    internal class ExchangeServiceTest
    {
        private Mock<IPortfolioRepository> portfolioService;
        private Mock<IStockService> stockService;
        private Mock<IPositionService> positionService;
        private Mock<IVwdService> wdService;
        [SetUp]
        public void Setup()
        {
            portfolioService = new Mock<IPortfolioRepository>();

            stockService = new Mock<IStockService>();
            positionService = new Mock<IPositionService>();
            positionService.Setup(x => x.GetAsync("StockName", It.IsAny<int>())).ReturnsAsync
                (
                new Position()
                {
                    Bought = 100,
                    Contract = 20,
                    Id = 1,
                    Stock = new Stock() { Id = 1, Isin = "123", Name = "StockName", Symbol = "Symbol1" },
                    StockId = 0,
                });
            wdService = new Mock<IVwdService>();

        }

        public static IEnumerable<TestCaseData> ConsolidatePositionTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new PositionRequest { BuyPrice = 100, Contract = 2 },
                    new Position { Bought = 100, Contract = 10 },
                    new Position { Bought = 200, Contract = 12 }).SetName("testcase1");
                yield return new TestCaseData(
                    new PositionRequest { BuyPrice = 25, Contract = 1 },
                    new Position { Bought = 100, Contract = 22 },
                    new Position { Bought = 125, Contract = 23 }).SetName("testcase2");
                yield return new TestCaseData(
                    new PositionRequest { BuyPrice = 55, Contract = 22 },
                    new Position { Bought = 55, Contract = 10 },
                    new Position { Bought = 110, Contract = 32 }).SetName("testcase3");
            }
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfSellMoreThanItHave()
        {
            var exchangeService = new ExchangeService(portfolioService.Object, stockService.Object,
                positionService.Object, wdService.Object);
            Assert.ThrowsAsync<Exception>(() => exchangeService.Sell(
               new SellRequest()
               { Contract = 21, Price = 10, Symbol = "StockName", UserId = 1 }));
        }
        [Test]
        public void Sell_ShouldThrowExceptionIfSymbolNotFound()
        {
            var exchangeService = new ExchangeService(portfolioService.Object, stockService.Object,
              positionService.Object, wdService.Object);
            Assert.ThrowsAsync<Exception>(() => exchangeService.Sell(
               new SellRequest()
               { Contract = 1, Price = 10, Symbol = "UnKnownStockName", UserId = 1 }));
        }
        [Test]
        public void Sell_TheSellAmountShouldBeSubtractFromInitBought()
        {
            var returnPosition = new Position() 
            {
                Contract = 19,
            }; positionService.Setup(x => x.UpdateAsync(It.IsAny<Position>())).ReturnsAsync(
                 returnPosition
                 );
            var exchangeService = new ExchangeService(portfolioService.Object, stockService.Object,
             positionService.Object, wdService.Object);
            var result = exchangeService.Sell(
               new SellRequest()
               { Contract = 1, Price = 10, Symbol = "StockName", UserId = 1 }).Result;
            
            Assert.AreEqual(result.Contract, 19);
        }
        [Test]
        public void Sell_NumberOfShouldBeAsExcpect()
        {

        }

        [TestCaseSource("ConsolidatePositionTestCases")]
        public void ConsolidatePosition_ShouldReturnExpectedResult(PositionRequest newPosition, Position oldPosition, Position expected)
        {
            var _portfolioService = new ExchangeService(portfolioService.Object, stockService.Object,
             positionService.Object, wdService.Object);
            var yeild = _portfolioService.ConsolidatePosition(newPosition, oldPosition);
            Assert.AreEqual(yeild.Bought, expected.Bought);
            Assert.AreEqual(yeild.Contract, expected.Contract);
        }
    }
}
