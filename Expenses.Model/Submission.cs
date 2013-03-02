using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public class Submission
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Employee Employee { get; set; }

        virtual public List<Expense> Expenses { get; set; }

    }
}
