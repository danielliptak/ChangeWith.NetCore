using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;

namespace Change.Db
{

    public class InMemoryDb : DbContext
    {

        public InMemoryDb(DbContextOptions<InMemoryDb> options) 
            : base(options)
        {
        }

        public DbSet<EcbEnvelope> RateOfCurrencies { get; set; }

    }
}
