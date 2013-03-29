using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Data.Contracts
{
    public interface ICurrencyProvider
    {
        float GetExchangeRate(string baseCurrency, string targetCurrency, DateTime day);
    }
}
