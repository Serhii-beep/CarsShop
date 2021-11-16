using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using CarShop.Models;
using CarShop.FileManager;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace CarShop
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbCarShopContext>(options => options.UseSqlServer(connection));


            string connectionIdentity = Configuration.GetConnectionString("IdentityConnection");
            //services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionIdentity));
            services.AddControllersWithViews();
            services.AddCors(options =>
            {
                options.AddPolicy("AllOrigins", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });
            //services.AddIdentity<User, IdentityRole>(opts =>
            //{
            //    opts.Password.RequiredLength = 5;
            //    opts.Password.RequireNonAlphanumeric = false;
            //    opts.Password.RequireLowercase = false;
            //    opts.Password.RequireUppercase = false;
            //    opts.Password.RequireDigit = false;
            //    opts.Password.RequiredUniqueChars = 0;
            //}).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/google-login";
            })
            .AddGoogle(options =>
            {
                //IConfigurationSection googleAuthNSection =
                //    Configuration.GetSection("Authentication:Google");

                options.ClientId = "886457074364-uls5i7hpero1p0jt66mojviktpctq8vu.apps.googleusercontent.com"; //googleAuthNSection["ClientId"];
                options.ClientSecret = "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"; //googleAuthNSection["ClientSecret"];
                //options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            });

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IFileManager, PhotoManager>();

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
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Cars}/{action=Index}/{categoryId?}");
            });
        }
    }
}
