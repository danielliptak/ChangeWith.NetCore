using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Change.Controllers;
using Microsoft.AspNetCore.Hosting.Internal;

namespace Change.Db
{
    public class ChangeResult
    {

        public decimal ExchangeResult { get; set; }
        public List<string> RateHistory { get; set; }

    }
}
