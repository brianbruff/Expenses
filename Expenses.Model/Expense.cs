using System;


namespace Expenses.Model
{
    public class Expense
    {   
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        virtual public byte[] Image { get; set; }

        public int CurrencyId { get; set; }
        virtual public Currency Currency { get; set; }

        virtual public ExpenseReport ExpenseReport { get; set; }

        public int TypeId { get; set; }
        virtual public ExpenseType Type { get; set; }

    }
}
