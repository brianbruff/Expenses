using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpensesTests
{
    [TestClass]
    public class DatabaseTests : IDisposable
    {


        [TestInitialize]
        public void TestCreate()
        {
            Database.SetInitializer(new BasicContentInitializer());
            using (var db = new Expenses.Data.ExpensesDbContext())
            {
                var clients = db.Clients;
                Assert.AreEqual(1, db.Clients.Count(), "We should have one client");

                
            }
        }

        //[TestMethod]
        //public async Task TestExpenses()
        //{

        //}


        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }

        private Expenses.Data.ExpensesDbContext _ctx = new Expenses.Data.ExpensesDbContext();
        
    }
}
