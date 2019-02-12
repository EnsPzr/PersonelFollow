using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonelFollow.Core.Abstract;
using PersonelFollow.Core.Concrete.EntityFramework;
using PersonelFollow.WebUI.Filter;
using PersonelFollow.WebUI.Middlewares;
using PersonelFollow.WebUI.Services.Session;

namespace PersonelFollow.WebUI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddTransient<LoginFilter>();
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(@"Server=Eness;Database=PersonelTracking;Trusted_Connection=true", b => b.MigrationsAssembly("PersonelFollow.WebUI")));
            services.AddTransient<IActiviyRepository, EfActivityRepository>();
            services.AddTransient<IMyActivityFollowRepository, EfMyActivityRepository>();
            services.AddTransient<IUserRepository, EfUserRepository>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStatusCodePages();
            app.UseFileServer();
            app.UseNodeModules(env.ContentRootPath);
            app.UseCookiePolicy();
            SeedData.Seed(app);
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Aktivitelerim}/{id?}");
            });
        }
    }
}
