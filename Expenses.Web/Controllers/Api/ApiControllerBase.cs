using Expenses.Data.Contracts;
using System.Web.Http;

namespace Expenses.Web.Controllers.Api
{
    public class ApiControllerBase : ApiController
    {
        protected IExpensesUow Uow { get; set; }
    }
}