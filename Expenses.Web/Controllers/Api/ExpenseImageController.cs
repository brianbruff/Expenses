using System;
using System.Data.Entity.Infrastructure;
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
    public class ExpenseImageController : ApiControllerBase
    {
        public ExpenseImageController(IExpensesUow uow)
            : base(uow)
        {
            
        }
        
        public ExpenseDto GetExpenseImage(int id)
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
                Image = expense.Image
            };
        }

        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage PutExpenseImage(int id, ExpenseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != dto.ExpenseId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var existingExpense = Uow.Expenses.Include(e => e.ExpenseReport.Employee).GetById(id);
            if (existingExpense.ExpenseReport.Employee.UserId != User.Identity.Name){
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            // We only update images in this controller
            existingExpense.Image = dto.Image;

            try
            {
                Uow.Expenses.Update(existingExpense);
                Uow.Commit();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
    }
}