using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;

namespace Expenses.Data.Contracts
{
    interface IExpensesUow
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        IRepository<Employee> Employees { get; }
    }
}