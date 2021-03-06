﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;

namespace Expenses.Model
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual Currency BaseCurrency { get; set; }

        public int BaseCurrencyId { get; set; }

        //public virtual List<Expense> Expenses { get; set; }

        public virtual List<ExpenseReport> ExpenseReports { get; set; }
    }
}
