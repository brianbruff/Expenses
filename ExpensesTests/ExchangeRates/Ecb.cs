using System;
using System.Threading.Tasks;
using Expenses.CurrencyProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpensesTests.ExchangeRates
{
    [TestClass]
    public class Ecb
    {
        [TestMethod]
        public async Task TestNoConversion()
        {
            // arrange
            var provider = new ECBCurrencyProvider();
            // act
            var result = await provider.GetExchangeRate("EUR", "EUR", DateTime.Now.Date);
            // assert
            Assert.AreEqual(1, result, "1 should have been returned");
        }

        [TestMethod]
        public async Task TestEuroBase()
        {
            // arrange
            var provider = new ECBCurrencyProvider();
            // act
            var result = await provider.GetExchangeRate("EUR", "GBP", DateTime.Now.Date);
            // assert
            Assert.AreNotEqual(1, result, "1 should not have been returned");
        }


    }
}
