using Expenses.Data;
using Expenses.Data.Contracts;
using System.Web.Http;

namespace Expenses.Web.Controllers.Api
{
    public class ApiControllerBase : ApiController
    {
        public ApiControllerBase(IExpensesUow uow)
        {
            this.Uow = uow;
        }

        
        protected IExpensesUow Uow { get; set; }
    }
}