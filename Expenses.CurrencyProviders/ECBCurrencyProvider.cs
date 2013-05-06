using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Expenses.Data.Contracts;

namespace Expenses.CurrencyProviders
{
    public class ECBCurrencyProvider : ICurrencyProvider
    {
        public async Task<decimal> GetExchangeRate(string fromCurrency, string targetCurrency, DateTime day)
        {
            const string dailyRateUri = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
            // ECB only gives ur daily rates in the feed, so we are a bit limited with this provider

            if (string.Compare(fromCurrency, targetCurrency, 
                System.StringComparison.OrdinalIgnoreCase) == 0)
                return 1;

            var xml = await new HttpClient().GetStringAsync(dailyRateUri);
            var doc = XDocument.Parse(xml);

            var fromRate = 1.0m;
            if (fromCurrency.ToUpper() != "EUR")
                fromRate = GetRate(doc, fromCurrency.ToUpper());

            var toRate = 1.0m;
            if (targetCurrency.ToUpper() != "EUR")
                toRate = GetRate(doc, targetCurrency.ToUpper());

            // todo: check cross rates and inversions 
            return fromRate / toRate;
        }

        

        private decimal GetRate(XDocument doc, string isoCode)
        {
            var ns = doc.Root.GetDefaultNamespace();
            var rates = doc.Descendants(ns + "Cube");
            
            var rate = rates.FirstOrDefault(r =>
            {
                if (r.HasAttributes)
                {
                    var currency = r.Attribute("currency");
                    if (currency != null && currency.Value.ToUpper() == isoCode)
                        return true;
                }
                return false;
            });

            if (rate == null)
                throw new Exception("Unable to find requested rate in ECB: " + isoCode);

            return decimal.Parse(rate.Attribute("rate").Value);
        }
    }
}
