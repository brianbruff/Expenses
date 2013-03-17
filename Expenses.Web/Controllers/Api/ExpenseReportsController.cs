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
        public IQueryable<ExpenseReportDto> GetExpenseReports()
        {
            return Uow.ExpenseReports.Include(r => r.Expenses).GetAll()
                .Where(r => r.Employee.UserId == User.Identity.Name)
                .Select(r => new ExpenseReportDto
                                 {
                                     ExpenseReportId = r.Id, 
                                     Name = r.Name, 
                                     Date = r.Date, 
                                     Expenses = r.Expenses.Select(e => new ExpenseDto
                                                                           {
                                                                               ExpenseId = e.Id,
                                                                               ExpenseReportId = r.Id,
                                                                               Date = e.Date,
                                                                               Description = e.Description,
                                                                               CurrencyId = e.CurrencyId,
                                                                               TypeId = e.TypeId,
                                                                               Image = e.Image
                                                                           }).AsQueryable()
                                 });
        }

        public IQueryable<Expense> GetExpenseReport(int id)
        {
            var expense = Uow.ExpenseReports.Include(e => e.Employee);
            if (expense == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            //if (expense.Employee.UserId != User.Identity.Name)
            //{
            //    // Trying to modify a record that does not belong to the user
            //    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            //}
            return null;
        }

        
    }
}