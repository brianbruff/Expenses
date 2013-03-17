using System;
using Expenses.Model;

namespace Expenses.Web.Models

{
    public class ExpenseReportDto
    {
        public int ExpenseReportId { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public int EmployeeId { get; set; }

        public ExpenseReport ToEntity()
        {
            return new ExpenseReport
                       {
                           Id = ExpenseReportId,
                           Name = Name,
                           Date = Date
                       };
        }
    }
}
