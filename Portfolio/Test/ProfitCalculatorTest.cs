using NUnit.Framework;
using Service.Implementation;
using System;
using System.Collections.Generic;
using Abstraction.Models;
using FluentAssertions;

namespace Test
{
    public class ProfitCalculatorTest
    {
        [TestCaseSource(nameof(ShouldSuccessProfit))]
        public void CalcProfit_ShouldReturnExpectedResult(List<Position> positions, double currentPrice, double expectedProfit)
        {
            //arrange
            var portfolioService = new ProfitCalculator();

            //act
            var profit = portfolioService.CalcTotalProfit(positions, currentPrice);

            //assert
            profit.Should().Be(expectedProfit);

        }

        [Test]
        public void CalcTotalProfit_ThrowExceptionIfListEmpty()
        {
            //arrange
            var portfolioService = new ProfitCalculator();

            //act and assert
            Assert.Throws<ArgumentNullException>(() => portfolioService.CalcTotalProfit(new List<Position>(), 0));
        }

        public static IEnumerable<object[]> ShouldSuccessProfit =>
            new List<object[]>
            {
                new object[] { new List<Position>()
                {
                    new Position()
                    {
                        TransactionType = TransactionType.Buy,
                        Price = 10,
                        Contract = 5,
                        CreateAt = DateTime.Now.AddDays(-20)
                    }
                }, 20, 100 },
                new object[] { new List<Position>()
                {
                    new()
                    {
                        TransactionType = TransactionType.Buy,
                        Price = 10,
                        Contract = 5,
                        CreateAt = DateTime.Now.AddDays(-20)
                    },
                    new()
                    {
                        TransactionType = TransactionType.Buy,
                        Price = 15,
                        Contract = 3,
                        CreateAt = DateTime.Now.AddDays(-15)
                    },
                    new()
                    {
                        TransactionType = TransactionType.Sell,
                        Price = 15,
                        Contract = 4,
                        CreateAt = DateTime.Now.AddDays(-10)
                    },
                    new()
                    {
                        TransactionType = TransactionType.Buy,
                        Price = 15,
                        Contract = 5,
                        CreateAt = DateTime.Now.AddDays(-1)
                    }
                }, 20, 63.64  },

                new object[] { new List<Position>()
                {
                    new()
                    {
                        TransactionType = TransactionType.Buy,
                        Price = 10,
                        Contract = 5,
                        CreateAt = DateTime.Now.AddDays(-20)
                    }
                }, 5, -50  }

            };
    }
}
