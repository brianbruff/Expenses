using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Data.Contracts;
using Expenses.Data.Helpers;
using Expenses.Model;

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


        public IRepository<Expense> Expenses { get { return GetStandardRepo<Expense>(); } }

       

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
