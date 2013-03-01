using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;

namespace Expenses.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public Currency BaseCurrency { get; set; }
        virtual public List<Expense> Expenses { get; set; }
    }
}
