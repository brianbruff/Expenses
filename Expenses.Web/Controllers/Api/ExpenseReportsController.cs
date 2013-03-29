using Expenses.Data.Contracts;
using Expenses.Model;
using Expenses.Web.Filters;
using Expenses.Web.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expenses.Web.Controllers.Api
{
    [Authorize]
    [ValidateHttpAntiForgeryToken]
    public class ExpenseReportsController : ApiControllerBase
    {
        public ExpenseReportsController(IExpensesUow uow): base(uow)
        {
            
        }

        public IQueryable<ExpenseReportDto> GetExpenseReports()
        {
            var res = Uow.ExpenseReports.Include(r => r.Employee).GetAll()
                .Where(r => r.Employee.UserId == User.Identity.Name)
                .Select(r => new ExpenseReportDto
                {
                    ExpenseReportId = r.Id,
                    Name = r.Name,
                    Date = r.Date
                });
            return res;
        }

        public ExpenseReportDto GetExpenseReport(int id)
        {
            var expenseReport = Uow.ExpenseReports.Include(e => e.Employee).Include(r => r.Expenses).GetById(id);
            if (expenseReport == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            if (expenseReport.Employee.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            var dto = new ExpenseReportDto
                                 {
                                     ExpenseReportId = expenseReport.Id,
                                     Name = expenseReport.Name,
                                     Date = expenseReport.Date,
                                     Expenses = expenseReport.Expenses.Select(e => new ExpenseDto
                                                                           {
                                                                               ExpenseId = e.Id,
                                                                               ExpenseReportId = expenseReport.Id,
                                                                               Date = e.Date,
                                                                               Description = e.Description,
                                                                               CurrencyId = e.CurrencyId,
                                                                               TypeId = e.TypeId,
                                                                               Amount = e.Amount
                                                                           }).AsQueryable()
                                 };

            return dto;

            
        }

        
    }
}