using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ExtCore.WebApplication.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Infrastructure;
using Microsoft.Extensions.FileProviders;
using System.IO;
using ExtCore.Data.EntityFramework;
using Secretaries.Data.Abstractions;
using Secretaries.Data.EntityFramework.Sqlite;
using Microsoft.AspNetCore.Mvc;
using Teachers.Data.Abstractions;
using Teachers.Data.EntityFramework.Sqlite;
using Microsoft.AspNetCore.Identity;
using Identity.Data.Entities;

namespace EducationalManagementSystem
{
    public class Startup
    {
        private string extensionsPath;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.Configuration = configuration;
            this.extensionsPath = webHostEnvironment.ContentRootPath + configuration["Extensions:Path"];
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddExtCore(this.extensionsPath);
            services.Configure<StorageContextOptions>(options =>
            {
                options.ConnectionString = this.Configuration.GetConnectionString("Default");
            }
             );

        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            
            applicationBuilder.UseDeveloperExceptionPage();

            applicationBuilder.UseExtCore();


            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
