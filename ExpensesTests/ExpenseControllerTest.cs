using System;
using Expenses.Data.Contracts;
using Expenses.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpensesTests
{
    [TestClass]
    public class ExpenseControllerTest
    {
        [TestMethod]
        public void TestGetById()
        {
            var expenseRepo = new Expenses.Data.Contracts.Fakes.StubIRepository<Expense>
                                  {
                                      GetByIdInt32 =
                                          (id) =>
                                          new Expense {Id = id}, 
                                  };
            var uow = new Expenses.Data.Contracts.Fakes.StubIExpensesUow
                          {
                              ExpensesGet = () => expenseRepo
                          };

            var ctrlr = new Expenses.Web.Controllers.Api.ExpensesController(uow);
            var expense = ctrlr.GetExpense(5);
            Assert.AreEqual(5, expense.Id);
            
             


        }
    }
}
