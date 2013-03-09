using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public class ExpenseType
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        virtual public List<Expense> Expenses { get; set; }
    }
}
