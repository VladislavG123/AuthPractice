using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthPractice.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthPractice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(x => x.UseSqlServer("Server=B-205-05; Database=AuthPractice; Trusted_Connection=True;"));
            services.AddScoped<UserContext>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.ToLower().Contains("service"))
                {
                    // поиск в бд человека с именем переданного через get запрос и сравнение значени€ с header-ом
                }
                await next.Invoke();
            });


            /*
              var browser = context.Request.Headers["User-Agent"].ToString();
            if (browser.ToLower().Contains("safari") || browser.ToLower().Contains("explorer"))
            {
                await context.Response.WriteAsync("ѕошел вон!");
            }
            await requestDelegate(context);
             */

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
