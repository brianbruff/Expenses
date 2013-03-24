using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Expenses.Data;
using Expenses.Data.Contracts;
using Expenses.Model;
using Expenses.Web.Filters;
using Expenses.Web.Models;

namespace Expenses.Web.Controllers.Api
{
    [Authorize]
    [ValidateHttpAntiForgeryToken]
    public class CurrenciesController : ApiControllerBase
    {
        public CurrenciesController(IExpensesUow uow)
            : base(uow)
        {
            
        }

        
        public IQueryable<Currency> GetCurrencies()
        {
            return Uow.Currencies.GetAll();
        }

        
    }
}