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
    [ValidateHttpAntiForgeryToken]
    public class ExpensesController : ApiControllerBase
    {
        public ExpensesController(IExpensesUow uow)
            : base(uow)
        {
            
        }
        

        public HttpResponseMessage PutExpense(int id, ExpenseDto dto)
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
            dto.UpdateEntity(existingExpense);
            if (existingExpense.ExpenseReport.Employee.UserId != User.Identity.Name){
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            // We don't update images in this controller
            existingExpense.Image = existingExpense.Image;

            try
            {
                Uow.Expenses.Update(existingExpense);
                Uow.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        public HttpResponseMessage PostExpense(ExpenseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var existingExpenseReport = Uow.ExpenseReports.Include(e => e.Employee).GetById(dto.ExpenseReportId);
            if (existingExpenseReport.Employee.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var expense = new Model.Expense { ExpenseReport = existingExpenseReport };
            dto.UpdateEntity(expense);

            try
            {
                Uow.Expenses.Add(expense);
                Uow.Commit();
                dto.ExpenseId = expense.Id;
            }
            catch (Exception exp)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created, dto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = dto.ExpenseId }));
            return response;
            
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
            
            try
            {
                Uow.Expenses.Delete(id);
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