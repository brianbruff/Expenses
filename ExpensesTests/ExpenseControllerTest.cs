using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Expenses.Data.Contracts;
using Expenses.Model;
using Expenses.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExpensesTests
{
    [TestClass]
    public class ExpenseControllerTest
    {
        [TestMethod]
        public void TestGetByIdMoq()
        {
            // Arrange
            var expenseRepo = new Mock<IRepository<Expense>>();
            expenseRepo.Setup(r => r.GetById(5)).Returns(
                new Expense
                {
                    Id = 5,
                    ExpenseReport = new ExpenseReport
                    {
                        Employee = new Employee { UserId = "TEST" }
                    }
                });
            expenseRepo.Setup(r => r.Include(x => x.ExpenseReport.Employee)).Returns(expenseRepo.Object);

            var uow = new Expenses.Data.Contracts.Fakes.StubIExpensesUow
            {
                ExpensesGet = () => expenseRepo.Object
            };

            var ctrlr = new Expenses.Web.Controllers.Api.ExpensesController(uow);
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            user.Setup(x => x.Identity).Returns(identity.Object);
            identity.Setup(x => x.Name).Returns("TEST");
            Thread.CurrentPrincipal = user.Object;
             
            // Act
            var response = ctrlr.PutExpense(1, new ExpenseDto { ExpenseId = 5});

            // Assert
            Assert.AreEqual(5, response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

    }
}
