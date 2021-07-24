using LuxoftCodeChallenge;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LuxoftCodeChallenge.UnitTest
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void GetChange_ValidAmounts_ReturnsTrue()
        {
            //Arrange
            var cashier = new Cashier
            {
                DenominationCount = 12,
                CountryCurrency = "USD",
                CurrencyDenomination = new decimal[] { 0.01M, 0.05M, 0.10M, 0.25M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M }
            };

            //Act
            var result = cashier.GetChange(10.50M);
            List<decimal> listResult = new() { 10.00M, 0.50M };

            //Assert
            Assert.AreEqual(listResult[0], result[0]);
            Assert.AreEqual(listResult[1], result[1]);
        }

        [TestMethod]
        public void GetChange_InvalidAmounts_ReturnsNotEqual()
        {
            //Arrange
            var cashier = new Cashier
            {
                DenominationCount = 12,
                CountryCurrency = "USD",
                CurrencyDenomination = new decimal[] { 0.01M, 0.05M, 0.10M, 0.25M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M }
            };

            List<decimal> listResult = new() { 5.00M, 5.00M, 0.50M };

            //Act
            var result = cashier.GetChange(10.50M);

            //Assert
            Assert.AreNotEqual(listResult[0], result[0]);
            Assert.AreNotEqual(listResult[1], result[1]);
            Assert.AreNotEqual(listResult[2], result.Count < listResult.Count ? 0.00M : result[2]);
        }

        [TestMethod]
        public void GetChange_InvalidParameter_ReturnsZero()
        {
            //Arrange
            var cashier = new Cashier
            {
                DenominationCount = 12,
                CountryCurrency = "USD",
                CurrencyDenomination = new decimal[] { 0.01M, 0.05M, 0.10M, 0.25M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M }
            };

            //Act
            var result = cashier.GetChange(-1);

            //Assert
            Assert.AreEqual(0, result.Count);

        }
    }
}
