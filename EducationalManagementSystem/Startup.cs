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
            // services.AddControllersWithViews();
            // services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddExtCore(this.extensionsPath);
            services.Configure<StorageContextOptions>(options =>
            {
                options.ConnectionString = this.Configuration.GetConnectionString("Default");
            }
             );

            //services.AddIdentity<>
            services.AddScoped<ISecretaryRepository, SecretaryRepository>();

        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseDeveloperExceptionPage();
            applicationBuilder.UseExtCore();
        }

        /*
        private string extensionsPath;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.extensionsPath = webHostEnvironment.ContentRootPath + configuration["Extensions:Path"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddExtCore(this.extensionsPath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseExtCore();
        }*/
    }
}
