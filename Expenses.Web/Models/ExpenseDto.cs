using System;
using Expenses.Model;

namespace Expenses.Web.Models

{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }

        public int ExpenseReportId { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int CurrencyId { get; set; }

        public int TypeId { get; set; }

        public byte[] Image { get; set; }

        public double Amount { get; set; }

        public Expense ToEntity()
        {
            return new Expense
                       {
                           Id = ExpenseId,
                           CurrencyId = CurrencyId,
                           Description = Description,
                           TypeId = TypeId,
                           Amount = Amount,
                           Image = Image,
                           Date = Date
                       };
        }
    }
}
