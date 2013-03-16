using Expenses.Data;
using Expenses.Data.Contracts;
using System.Web.Http;

namespace Expenses.Web.Controllers.Api
{
    public class ApiControllerBase : ApiController
    {
        public ApiControllerBase()
        {
            var resolver = GlobalConfiguration.Configuration.DependencyResolver;
            Uow = resolver.GetService(typeof(IExpensesUow)) as IExpensesUow;
        }

        
        protected IExpensesUow Uow { get; set; }
    }
}