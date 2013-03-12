using System;
using System.Data.Entity.ModelConfiguration;


namespace Expenses.Model

{
    public class ExpenseDto
    {   
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public int CurrencyId { get; set; }
        
        public int TypeId { get; set; }
        
    }
}
