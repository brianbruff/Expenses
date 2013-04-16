using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Expenses.Data.Contracts;
using Expenses.Web.Filters;
using Expenses.Web.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expenses.Web.Controllers.Api
{
    //[System.Web.Http.Authorize]
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
            if (existingExpense.ExpenseReport.Employee.UserId != User.Identity.Name)
            {
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
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        //[System.Web.Http.HttpPost]
        //public async Task<HttpResponseMessage> UploadFile()
        //{
        //    // Check if the request contains multipart/form-data.
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        var sb = new StringBuilder(); // Holds the response body

        //        // Read the form data and return an async task.
        //        var r = await Request.Content.ReadAsMultipartAsync(provider);

        //        // This illustrates how to get the form data.
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            foreach (var val in provider.FormData.GetValues(key))
        //            {
        //                sb.Append(string.Format("{0}: {1}\n", key, val));
        //            }
        //        }

        //        var expenseId = int.Parse(provider.FormData.GetValues("expenseId").First());


        //        // This illustrates how to get the file names for uploaded files.
        //        foreach (var file in provider.FileData)
        //        {
        //            FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    


        //            using (var ms = new MemoryStream())
        //            {
        //                var bm = new Bitmap(file.LocalFileName);
        //                bm.Save(ms, ImageFormat.Jpeg);


        //                var expense = Uow.Expenses.Include(e => e.ExpenseReport.Employee).GetById(expenseId);
        //                expense.Image = ms.ToArray();
        //                Uow.Commit();
        //            }

        //        }
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(sb.ToString())
        //        };
        //    }
        //    catch (System.Exception e)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //    }
        //}


    }
}