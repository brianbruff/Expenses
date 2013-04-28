using Expenses.Data.Contracts;
using Expenses.Web.Models;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Expenses.Web.Controllers.Api
{
    //[System.Web.Http.Authorize]
    public class ExpenseImageController : ApiControllerBase
    {
        public ExpenseImageController(IExpensesUow uow)
            : base(uow)
        {

        }
        
        public HttpResponseMessage GetExpenseImage(int id)
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

            var dto = new ExpenseDto
            {
                Image = expense.Image,
                ImageType = expense.ImageType
            };

            var response = Request.CreateResponse(HttpStatusCode.Created, dto);
            //response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { MaxAge = new System.TimeSpan(0, 10, 0) };
            return response;
        }

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
            if (existingExpense.ExpenseReport.Employee.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            // We only update images in this controller
            existingExpense.Image = dto.Image;
            existingExpense.ImageType = dto.ImageType;

            try
            {
                Uow.Expenses.Update(existingExpense);
                Uow.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DeleteExpense(int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            
            var existingExpense = Uow.Expenses.Include(e => e.ExpenseReport.Employee).GetById(id);
            if (existingExpense.ExpenseReport.Employee.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            // We only update images in this controller
            existingExpense.Image = null;
            existingExpense.ImageType = null;

            try
            {
                Uow.Expenses.Update(existingExpense);
                Uow.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, id);
        }
    }
}