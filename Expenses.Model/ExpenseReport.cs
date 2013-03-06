using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public class ExpenseReport
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }

        public int EmployeeId { get; set; }

        public virtual List<Expense> Expenses { get; set; }
    }
}
