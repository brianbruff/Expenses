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

        public string ImageType { get; set; }

        public byte[] Image { get; set; }

        public double Amount { get; set; }

        public float ExchangeRate { get; set; }


        public void UpdateEntity(Expense expense)
        {
            expense.CurrencyId = CurrencyId;
            expense.Currency = null;
            expense.Description = Description;
            expense.Description = Description;
            expense.TypeId = TypeId;
            expense.Amount = Amount;
            expense.ExchangeRate = ExchangeRate;
            expense.Date = Date;
        }
    }
}
