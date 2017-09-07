using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using Change.Db;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Change.Controllers
{

    public class BaseOfExchange
    {
        public string date { get; set; }
        public string amount { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }

    [Route("api/[controller]")]
    public class XMLController : Controller
    {

        private InMemoryDb Context;

        public XMLController(InMemoryDb db)
        {
            this.Context = db;
        }

        [HttpGet("[action]/all")]
        public async Task<List<EcbEnvelope.CubeRoot>> GetCurrencies()
        {
            var test = await Context.RateOfCurrencies.SelectMany(i => i.CubeRootEl)
                                            .Include(i => i.CubeItems)
                                            .OrderByDescending(item => item.Time)
                                            .ToListAsync();
            return test;
        }

        [HttpPost("[action]")]
        public async Task<ChangeResult> Exchange([FromBody] BaseOfExchange baseOfExchange)
        {
            var rateOfFrom = await Context.RateOfCurrencies.SelectMany(i => i.CubeRootEl)
                                            .Include(i => i.CubeItems)
                                            .Where(i => i.Time == baseOfExchange.date)
                                            .SelectMany(i => i.CubeItems)
                                            .Where(i => i.Currency == baseOfExchange.from)
                                            .Select(i => i.RateStr)
                                            .FirstOrDefaultAsync();
            rateOfFrom = rateOfFrom.Replace(".", ",");

            var rateOfTo = await Context.RateOfCurrencies.SelectMany(i => i.CubeRootEl)
                .Include(i => i.CubeItems)
                .Where(i => i.Time == baseOfExchange.date)
                .SelectMany(i => i.CubeItems)
                .Where(i => i.Currency == baseOfExchange.to.ToString())
                .Select(i => i.RateStr)
                .FirstOrDefaultAsync();
            rateOfTo = rateOfTo.Replace(".", ",");

            var currencyHistory = await Context.RateOfCurrencies
                .SelectMany(i => i.CubeRootEl)
                .SelectMany(i => i.CubeItems).Where(i => i.Currency == baseOfExchange.to)
                .Select(i => i.RateStr)
                .ToListAsync();

            var changeResult = new ChangeResult();

            decimal rateToAsDecimal = 0;
            decimal rateFromAsDecimal = 0;
            decimal amountAsDecimal = 0;

            try
            {
                rateFromAsDecimal = Decimal.Parse(rateOfFrom, NumberStyles.AllowDecimalPoint);
                rateToAsDecimal = Decimal.Parse(rateOfTo, NumberStyles.AllowDecimalPoint);
                amountAsDecimal = Decimal.Parse(baseOfExchange.amount, NumberStyles.AllowDecimalPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            changeResult.ExchangeResult = rateToAsDecimal /
                                           rateFromAsDecimal *
                                           amountAsDecimal;

            changeResult.RateHistory = currencyHistory;

            return changeResult;
        }


    }
}
