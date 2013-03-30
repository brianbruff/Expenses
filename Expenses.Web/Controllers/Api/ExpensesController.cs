﻿using System.Data.Entity.Infrastructure;
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

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
    }
}