using Expenses.Data;
using Expenses.Data.Contracts;
using System.Web.Http;

namespace Expenses.Web.Controllers.Api
{
    public class ApiControllerBase : ApiController
    {
        // don't want to have new constructor in each derived class but can't currently see a way around without coupling in the concret classs
        // 

        //public ApiControllerBase()
        //{
        //    var resolver = GlobalConfiguration.Configuration.DependencyResolver;
        //    Uow = resolver.GetService(typeof(ExpensesUow)) as ExpensesUow;
        //}

        
        protected IExpensesUow Uow { get; set; }
    }
}