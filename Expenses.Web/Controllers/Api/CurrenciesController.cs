using Expenses.Data.Contracts;
using Expenses.Model;
using Expenses.Web.Filters;
using System.Linq;
using System.Web.Http;

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