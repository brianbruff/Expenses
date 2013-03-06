using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpensesTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void TestCreate()
        {
            Database.SetInitializer<Expenses.Data.ExpensesDbContext>(new Expenses.Data.ExpensesDbContext());
            using (var db = new Expenses.Data.ExpensesDbContext())
            {
                var clients = db.Clients;
                
                Assert.AreEqual(1, db.Clients.Count(), "We should have one client");
                
            }
        }
    }
}
