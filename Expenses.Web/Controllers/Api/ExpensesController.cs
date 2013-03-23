using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Expenses.Data;
using Expenses.Data.Contracts;
using Expenses.Model;
using Expenses.Web.Models;

namespace Expenses.Web.Controllers.Api
{
    [Authorize]
    //[ValidateHttpAntiForgeryToken]
    public class ExpensesController : ApiControllerBase
    {
        public ExpensesController(IExpensesUow uow)
            : base(uow)
        {
            
        }

        [Authorize(Roles="Admin")]
        public IQueryable<ExpenseDto> GetExpenses()
        {
            return Uow.Expenses.Include(e => e.ExpenseReport.Employee).GetAll()
                .Select(e => new ExpenseDto
                {
                    ExpenseId = e.Id,
                    ExpenseReportId = e.ExpenseReport.Id,
                    Date = e.Date,
                    Description = e.Description,
                    CurrencyId = e.CurrencyId,
                    TypeId = e.TypeId,
                });
        }

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
                Image = expense.Image
            };
        }

        // PUT api/Todo/5
        //public HttpResponseMessage PutExpense(int id, ExpenseDto expenseDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    if (id != expenseDto.Id)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    //Expense expense = expenseDto.ToEntity();
        //    //ExpenseReport ExpenseReport = db.ExpenseReports.Find(expense.ExpenseReportId);
        //    //if (ExpenseReport == null)
        //    //{
        //    //    return Request.CreateResponse(HttpStatusCode.NotFound);
        //    //}

        //    //if (ExpenseReport.UserId != User.Identity.Name)
        //    //{
        //    //    // Trying to modify a record that does not belong to the user
        //    //    return Request.CreateResponse(HttpStatusCode.Unauthorized);
        //    //}

        //    // Need to detach to avoid duplicate primary key exception when SaveChanges is called
        //    //db.Entry(ExpenseReport).State = EntityState.Detached;
        //    //db.Entry(expense).State = EntityState.Modified;

        //    try
        //    {
        //        //db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        //// POST api/Todo
        //public HttpResponseMessage PostExpense(ExpenseDto expenseDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    //ExpenseReport ExpenseReport = db.ExpenseReports.Find(expenseDto.ExpenseReportId);
        //    //if (ExpenseReport == null)
        //    //{
        //    //    return Request.CreateResponse(HttpStatusCode.NotFound);
        //    //}

        //    //if (ExpenseReport.UserId != User.Identity.Name)
        //    //{
        //    //    // Trying to add a record that does not belong to the user
        //    //    return Request.CreateResponse(HttpStatusCode.Unauthorized);
        //    //}

        //    //Expense expense = expenseDto.ToEntity();

        //    //// Need to detach to avoid loop reference exception during JSON serialization
        //    //db.Entry(ExpenseReport).State = EntityState.Detached;
        //    //db.Expenses.Add(expense);
        //    //db.SaveChanges();
        //    //expenseDto.ExpenseId = expense.ExpenseId;

        //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, expenseDto);
        //    //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = expenseDto.ExpenseId }));
        //    return response;
        //}

        // DELETE api/Todo/5
        //public HttpResponseMessage DeleteExpense(int id)
        //{
            //Expense expense = db.Expenses.Find(id);
            //if (expense == null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.NotFound);
            //}

            //if (db.Entry(expense.ExpenseReport).Entity.UserId != User.Identity.Name)
            //{
            //    // Trying to delete a record that does not belong to the user
            //    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            //}

            //ExpenseDto expenseDto = new ExpenseDto(expense);
            //db.Expenses.Remove(expense);

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError);
            //}

        //    return Request.CreateResponse(HttpStatusCode.OK, expenseDto);
        //}

        protected override void Dispose(bool disposing)
        {
           // db.Dispose();
            base.Dispose(disposing);
        }
    }
}