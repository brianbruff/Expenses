using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Data.Contracts
{
    public interface ICurrencyProvider
    {
        Task<decimal> GetExchangeRate(string fromCurrency, string targetCurrency, DateTime day);
    }
}
