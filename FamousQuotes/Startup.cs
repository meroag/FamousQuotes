using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuotes.Auth;
using FamousQuotes.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FamousQuotes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/errorsLog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddRazorPages();
            services.AddDbContext<MyDbContext>(c => c.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddAuthentication(TokenAuthentication.SchemeName)
                .AddScheme<TokenAuthenticationOptions, TokenAuthentication>
                    (TokenAuthentication.SchemeName, o =>{});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
