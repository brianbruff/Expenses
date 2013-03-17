using Expenses.Data.Contracts;
using Expenses.Data.Helpers;
using Expenses.Model;
using System;

namespace Expenses.Data
{
   
    public class ExpensesUow : IExpensesUow, IDisposable
    {
        public ExpensesUow(RepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }


        // Repositories
        public IRepository<Currency> Currencies { get { return GetStandardRepo<Currency>(); } }
        public IRepository<ExpenseType> ExpenseTypes { get { return GetStandardRepo<ExpenseType>(); } }
        public IRepository<Employee> Employees { get { return GetStandardRepo<Employee>(); } }
        public IRepository<Expense> Expenses { get { return GetStandardRepo<Expense>(); } }
        public IRepository<ExpenseReport> ExpenseReports { get { return GetStandardRepo<ExpenseReport>(); } }
       

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void Commit()
        {
            //System.Diagnostics.Debug.WriteLine("Committed");
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new ExpensesDbContext();

            // TODO: Set any options here

        }

        protected RepositoryProvider RepositoryProvider { get; set; }

        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private ExpensesDbContext DbContext { get; set; }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}
