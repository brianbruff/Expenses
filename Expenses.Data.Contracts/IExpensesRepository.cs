using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;

namespace Expenses.Data.Contracts
{
   
    public interface IExpensesRepository : IRepository<Expense>
    {
        //Publisher GetByCode(string code);
        
        
    }
}
