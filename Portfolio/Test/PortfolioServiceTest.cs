using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Moq;
using NUnit.Framework;
using Service.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public class PortfolioServiceTest
    {
        private Mock<IPortfolioRepository> _portfolioRepository;
        private Mock<IVwdService> _wdService;
        private Mock<IProfitCalculator> profitCalculator;
        [SetUp]
        public void Setup()
        {
            _portfolioRepository = new Mock<IPortfolioRepository>();
            _wdService = new Mock<IVwdService>();
            _portfolioRepository.Setup(x => x.GetPortfolioItems(It.IsAny<int>())).ReturnsAsync(new Portfolio
            {
                Name = "name",
                Id = 1,
                Positions = GetPostions(),
                UserId = 1,
            });
            _wdService.Setup(x => x.GetAsync(It.IsAny<List<string>>())).Returns(Task.FromResult<List<VwdResponse>>(
                new List<VwdResponse>(){ new VwdResponse
            {
                VwdKey="symbol1",
                Name = "stock1",
                Price = 80,
                Currency = "eur"
            },
                new VwdResponse
            {
                VwdKey="symbol2",
                Name = "stock2",
                Price = 200,
                Currency = "eur"
            }
                }));
 
            profitCalculator = new Mock<IProfitCalculator>();
            profitCalculator.Setup(x => x.CalcTotalProfit(It.IsAny<List<Position>>(), It.IsAny<double>())).Returns(1);

        }

        private List<Position> GetPostions()
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
                    CreateAt=System.DateTime.Now.AddDays(-1),
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
                    Bought=400,
                    Contract=3,
                    StockId=1,
                    CreateAt=System.DateTime.Now.AddDays(-2),
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
                    CreateAt=System.DateTime.MinValue,
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
        public void Get_ShouldNotBeNullInCaseThereIsNoPosition()
        {
            var _portfolioService = new PortfolioService(_portfolioRepository.Object, _wdService.Object, profitCalculator.Object);
            var protItem = _portfolioService.Get(1).Result;
            Assert.IsNotNull(protItem);
        }
        [Test]
        public void Get_ShouldReturnExpectedStockCount()
        {
            const int stockCount = 2;
            var _portfolioService = new PortfolioService(_portfolioRepository.Object, _wdService.Object, profitCalculator.Object);
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
                Yield=1,
                Current=400,
                Name="stock1",
                Price=80,
                Quantity=5,
                Symbol="symbol1",
               
            },
            new PortfolioItem{
                Bought= 900,
                Yield=1,
                Current=800,
                Name="stock2",
                Price=200,
                Quantity=4,
                Symbol="symbol2"
            }};
            var _portfolioService = new PortfolioService(_portfolioRepository.Object, _wdService.Object, profitCalculator.Object);
            var protItem = _portfolioService.Get(1).Result;
            CollectionAssert.AreEqual(protItem, expect);
        }

    }
}