using System;
using Expenses.Model;

namespace Expenses.Web.Models

{
    public class ExpenseReportDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public int EmployeeId { get; set; }

        public ExpenseReport ToEntity()
        {
            return new ExpenseReport
                       {
                           Id = Id,
                           Name = Name,
                           Date = Date
                       };
        }
    }
}
