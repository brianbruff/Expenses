using Expenses.Data.Contracts;
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
    public class ExpensesController : ApiControllerBase
    {
        public ExpensesController(IExpensesUow uow)
            : base(uow)
        {
            
        }

        //[Authorize(Roles="Admin")]
        //public IQueryable<ExpenseDto> GetExpenses()
        //{
        //    return Uow.Expenses.Include(e => e.ExpenseReport.Employee).GetAll()
        //        .Select(e => new ExpenseDto
        //        {
        //            ExpenseId = e.Id,
        //            ExpenseReportId = e.ExpenseReport.Id,
        //            Date = e.Date,
        //            Description = e.Description,
        //            CurrencyId = e.CurrencyId,
        //            TypeId = e.TypeId,
        //            Amount = e.Amount,
        //            ExchangeRate = e.ExchangeRate
        //        });
        //}

        public ExpenseDto GetExpense(int id)
        {
            var expense = Uow.Expenses.Include(e => e.ExpenseReport.Employee).GetById(id);
            if (expense == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            if (expense.ExpenseReport.Employee.UserId != User.Identity.Name)
            {
                // Trying to access a record that does not belong to the user
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            return new ExpenseDto
            {
                ExpenseId = expense.Id,
                ExpenseReportId = expense.Id,
                Date = expense.Date,
                Description = expense.Description,
                CurrencyId = expense.CurrencyId,
                TypeId = expense.TypeId,
                Image = expense.Image,
                Amount = expense.Amount,
                ExchangeRate = expense.ExchangeRate
            };
        }
        
    }
}