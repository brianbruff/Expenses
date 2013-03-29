using Expenses.Data.Contracts;
using Expenses.Web.Filters;
using System;
using System.Web.Http;

namespace Expenses.Web.Controllers.Api
{
    [ValidateHttpAntiForgeryToken]
    public class ExchangeRateController : ApiController
    {
        
        public ExchangeRateController(ICurrencyProvider currencyProvider)
        {
            _currencyProvider = currencyProvider;
        }
        
        public float Get(string baseIso, string targetIso, DateTime exchangeDate)
        {
            return _currencyProvider != null ? _currencyProvider.GetExchangeRate(baseIso, targetIso, exchangeDate) : 1;
        }


        private readonly ICurrencyProvider _currencyProvider = null;
    }
}
