using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
using LibraryData.Interface;
using LibraryServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement
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

            //Registering our Services which is the Interface created
            services.AddSingleton(Configuration);
            services.AddScoped<ILibraryAsset, LibraryAssestService>();
            services.AddScoped<ICheckOut, CheckOutServices>();
            services.AddScoped<IPatron, PatronService>();
            services.AddScoped<ILibraryBranch, LibraryBranchService>();
            services.AddScoped<ILibraryCard, LibraryCardService>();

            //registering the connection string
            var connectionString = Configuration["connectionStrings:LibraryDbConnectionString"];
            services.AddDbContextPool<LibraryDBContext>(options => options.UseSqlServer(connectionString));

         
            //Or we add connectionString this way
            //services.AddDbContextPool<LibraryDBContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("LibraryDbConnectionString")));

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            //use to serve static files such as image files etc
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
