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
            services.AddDbContext<UserContext>(x => x.UseNpgsql("Server=localhost; Database=Middleware; User ID = postgres; Password = postgres;"));
            services.AddScoped<UserContext>();
            services.AddMvc(x => x.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.ToLower().Contains("service"))
                {
                    // поиск в бд человека с именем переданного через get запрос и сравнение значения с header-ом
                    if (context.Request.Query.ContainsKey("name"))
                    {
                        string name = context.Request.Query["name"].ToString();

                        var userContext = context.RequestServices.GetService<UserContext>();

                        var users = (await userContext.Users.Where(user => user.Name == name).ToListAsync());
                        if (users.Count == 0) await context.Response.WriteAsync("Пошел вон!1");

                        var user = users[0];

                        if (!(user is null) && user.SecureCode == context.Request.Headers["Auth"].ToString())
                        {
                            await next.Invoke();
                        } 
                    }
                    else
                        await context.Response.WriteAsync("Пошел вон!2");
                }
                await next.Invoke();
            });


            /*
              var browser = context.Request.Headers["User-Agent"].ToString();
            if (browser.ToLower().Contains("safari") || browser.ToLower().Contains("explorer"))
            {
                await context.Response.WriteAsync("Пошел вон!");
            }
            await requestDelegate(context);
             */
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "api/{controller}/{action}/{id?}");
            });
        }
    }
}
