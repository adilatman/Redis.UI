using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Redis.UI.DAL;
using Redis.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.UI
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
            services.AddControllersWithViews();
            services.AddScoped<IPhotosRepo, PhotosRepo>();

            string mongoConnectionString = this.Configuration.GetConnectionString("MongoConnectionStringLocal");
            services.AddTransient(s => new PhotosMongo(mongoConnectionString, "Atman", "Photos"));
            services.AddScoped<IPhotosMongo>(_ => new PhotosMongo(mongoConnectionString, "Atman", "Photos"));

            services.AddStackExchangeRedisCache(options =>
            {
                string connection = Configuration.GetConnectionString("Redis");
                //options.InstanceName = "Redis";
                options.Configuration = connection;
            });
            
            services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(20));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Privacy}/{id?}");
            });
        }
    }
}
