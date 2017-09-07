using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Xml;
using Change.Db;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Change
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InMemoryDb>(obj => obj.UseInMemoryDatabase());
            services.AddMvc();

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //var context2 = serviceProvider.GetService<InMemoryDb>();
            var context = app.ApplicationServices.GetService<InMemoryDb>();
            FillUptDatabase(context);
            var test = context;

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            }
            );
        }


        private void FillUptDatabase(InMemoryDb context)
        {

            //WebProxy wp = new WebProxy("localhost:3128");
            //WebClient wc = new WebClient { Proxy = wp };
            //String urlString = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
            //MemoryStream ms = new MemoryStream(wc.DownloadData(urlString));
            //var reader = new XmlTextReader(ms);

            String urlString = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
            var reader = new XmlTextReader(urlString);

            EcbEnvelope obj = new XmlSerializer(typeof(EcbEnvelope)).Deserialize(reader) as EcbEnvelope;

            context.RateOfCurrencies.Attach(obj);
            context.SaveChanges();
  
        }
    }
}
