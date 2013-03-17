using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;

namespace Expenses.Data.Contracts
{
    public interface IExpensesUow
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        IRepository<ExpenseType> ExpenseTypes { get; }
        IRepository<Currency> Currencies { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Expense> Expenses { get; }
        IRepository<ExpenseReport> ExpenseReports { get; }
    }
}