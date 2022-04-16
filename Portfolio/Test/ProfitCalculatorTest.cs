using NUnit.Framework;
using Service.Implementation;
using System;

namespace Test
{
    public class ProfitCalculatorTest
    {

        [Test]
        [TestCase(10, 10, 0)]
        [TestCase(20, 10, -50)]
        [TestCase(20, 30, 50)]
        [TestCase(34, 42, 23.52)]
        [TestCase(90, 270, 200)]
        public void CalcYeild_ShouldReturnExpectedResult(double boughtPrice, double currentPrice, double expected)
        {
            var _portfolioService = new ProfitCalculator();
            var yeild = _portfolioService.CalcProfit(boughtPrice, currentPrice);
            Assert.AreEqual(expected, expected);
        }
        [Test]
        public void CalcTotalProfit_ThrowExceptionIfArgumentIsNull()
        {
            var _portfolioService = new ProfitCalculator();
            Assert.Throws<ArgumentNullException>(() => _portfolioService.CalcTotalProfit(null, 10));
        }
    }
}
