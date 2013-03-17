using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Expenses.Data;
using Expenses.Data.Contracts;
using Expenses.Model;
using Expenses.Web.Filters;
using Expenses.Web.Models;

namespace Expenses.Web.Controllers.Api
{
    [Authorize]
    [ValidateHttpAntiForgeryToken]
    public class ExpenseReportsController : ApiControllerBase
    {
        public IQueryable<ExpenseReportDto> GetExpenseReports()
        {
            return Uow.ExpenseReports.GetAll().Where(r => r.Employee.UserId == User.Identity.Name)
                .Select(x => new ExpenseReportDto{ ExpenseReportId = x.Id, Name = x.Name, Date = x.Date});
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