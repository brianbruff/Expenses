using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Expenses.Data.Contracts;
using Expenses.Model;
using Expenses.Web.Controllers.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExpensesTests
{
    [TestClass]
    public class CurrenciesControllerTest
    {
        [TestMethod]
        public void TestGetByIdMoq()
        {
            // Arrange
            var repo = new Mock<IRepository<Currency>>();
            repo.Setup(r => r.GetAll()).Returns(
                new List<Currency>
                {
                    new Currency
                    {
                        Id = 1,
                        Code = "GBP"
                    }, 
                    new Currency
                    {
                        Id = 2,
                        Code = "EUR"
                    }
                }.AsQueryable()
            );

            var uow = new Mock<IExpensesUow>();
            uow.SetupGet(u => u.Currencies).Returns(repo.Object);
            
            var ctrlr = new CurrenciesController(uow.Object);

            // Act
            var currencies = ctrlr.GetCurrencies();

            // Assert
            Assert.AreEqual(5, currencies.Count());
        }

    }
}
