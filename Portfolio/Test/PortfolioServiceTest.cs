using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Moq;
using NUnit.Framework;
using Service.Implementation;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class PortfolioServiceTest
    {
        private Mock<IPortfolioRepository> _portfolioRepository;
        private Mock<IVwdService> _wdService;
        [SetUp]
        public void Setup()
        {
            _portfolioRepository = new Mock<IPortfolioRepository>();
            _wdService = new Mock<IVwdService>();
            _portfolioRepository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(new Portfolio
            {
                Name = "name",
                Id = 1,
                Positions = GetPostions1(),
                UserId = 1,
                PostionId = 1,
            });
            _wdService.Setup(x => x.GetAsync("symbol1")).ReturnsAsync(new VwdResponse
            {
                Name = "stock1",
                Price = 80,
                Currency = "eur"
            });
            _wdService.Setup(x => x.GetAsync("symbol2")).ReturnsAsync(new VwdResponse
            {
                Name = "stock2",
                Price = 200,
                Currency = "eur"
            });
        }
        private List<Position> GetPostions2()
        {
            return new List<Position>
            {
            new Position() {
                    Id=3,
                    PortfolioId=2,
                    Bought=200,
                    Contract=5,
                    StockId=2,
                    Stock=new Stock
                    {
                    Id=2,
                    Isin="2",
                    Name="stockName2",
                    Symbol="symbol2"
                    }},
            new Position() {
                    Id=4,
                    PortfolioId=2,
                    Bought=100,
                    Contract=2,
                    StockId=1,
                    Stock=new Stock
                    {
                    Id=1,
                    Isin="123",
                    Name="stock1",
                    Symbol="symbol1"
                    }
            }
            };
        }
        private List<Position> GetPostions1()
        {
            return new List<Position>
            {
            new Position()
                    {
                    Id=1,
                    PortfolioId=1,
                    Bought=100,
                    Contract=2,
                    StockId=1,
                    Stock=new Stock
                    {
                    Id=1,
                    Isin="123",
                    Name="stock1",
                    Symbol="symbol1"
                    }},
            new Position()
                    {
                    Id=2,
                    PortfolioId=1,
                    Bought=900,
                    Contract=4,
                    StockId=2,
                    Stock=new Stock
                    {
                    Id=2,
                    Isin="2",
                    Name="stock2",
                    Symbol="symbol2"
                    }},
            };
        }
        [Test]
        public void Get_ShouldNotBeNull()
        {
            var _portfolioService = new PortfolioService(_portfolioRepository.Object, _wdService.Object);
            var protItem = _portfolioService.Get(1).Result;
            Assert.IsNotNull(protItem);
        }
        [Test]
        public void Get_ShouldReturnExpectedStockCount()
        {
            const int stockCount = 2;
            var _portfolioService = new PortfolioService(_portfolioRepository.Object, _wdService.Object);
            var protItem = _portfolioService.Get(1).Result;
            Assert.AreEqual(protItem.Count, stockCount);
        }
        [Test]
        public void Get_ShouldReturnExpectedValues()
        {
            List<PortfolioItem> expect = new List<PortfolioItem>
            {
            new PortfolioItem {
                Bought= 100,
                Yield=60,
                Current=160,
                Name="stock1",
                Price=80,
                Quantity=2,
                Symbol="symbol1"
            },
            new PortfolioItem{
                Bought= 900,
                Yield=-11.11,
                Current=800,
                Name="stock2",
                Price=200,
                Quantity=4,
                Symbol="symbol2"
            }};
            var _portfolioService = new PortfolioService(_portfolioRepository.Object, _wdService.Object);
            var protItem = _portfolioService.Get(1).Result;
            CollectionAssert.AreEqual(protItem, expect);
        }
        [Test]
        [TestCase(100, 10, 10, 0)]
        [TestCase(200, 10, 10, -50)]
        [TestCase(200, 30, 10, 50)]
        [TestCase(344, 42, 10, 22.09)]
        [TestCase(900, 270, 10, 200)]
        public void CalcYeild_ShouldReturnExpectedResult(double totalBoughtPrice, double ItemCurrentPrice, int quaintity, double expected)
        {
            var _portfolioService = new PortfolioService(_portfolioRepository.Object, _wdService.Object);
            var yeild = _portfolioService.CalcYeild(totalBoughtPrice, ItemCurrentPrice, quaintity);
            Assert.AreEqual(yeild, expected);
        }


   


    }
}