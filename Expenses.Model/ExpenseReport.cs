using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public class ExpenseReport
    {
        public ExpenseReport()
        {
            this.Expenses = new List<Expense>();    
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? Date { get; set; }

        virtual public Employee Employee { get; set; }

        public int EmployeeId { get; set; }

        virtual public List<Expense> Expenses { get; set; }
    }
}
