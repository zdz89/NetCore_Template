using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore_Template.Data;
using NetCore_Template.Data.Model;
using NetCore_Template.IoC;
using System;

namespace WebApp1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IoC.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add ApplicationDbContext to DI
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(IoC.Configuration.GetConnectionString("DefaultConnection"))
            );

            //Authentication - validate the given informations e.g. username in passed cookies
            //Authorization - process determines what user is able to do

            // This method adds validated user to HttpContext.User
            // Adds UserManager, RoleManager, SignInManager...
            // Adds user identity with cookie authentication
            // https://github.com/aspnet/Identity/blob/master/src/Identity/IdentityServiceCollectionExtensions.cs
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                // Adds UserStore and RoleStore needs in UserManager and RoleManager
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // Generates unique keys and hashes e.g. forgot password links
                .AddDefaultTokenProviders();

            // Identity options
            services.Configure<IdentityOptions>(options =>
            {
                //Password policy
                // TODO: change it
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 2;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            });

            // Cookies options
            services.ConfigureApplicationCookie(options =>
            {
                // Set login redirect URL
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                // Cookie expire time
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            CheckDatabaseCreatedAndMigrated(serviceProvider);
            SeedDatabase(serviceProvider);
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SeedDatabase(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (SeedRoles(roleManager))
                SeedUsers(userManager);
        }

        private bool SeedUsers(UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (userManager.FindByNameAsync("Admin").Result == null)
                {
                    ApplicationUser admin = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "admini@speedmail.pl"
                    };

                    var result = userManager.CreateAsync(admin, "password").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "Admin").Wait();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            try
            {
                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    ApplicationRole role = new ApplicationRole();
                    role.Name = "Admin";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void CheckDatabaseCreatedAndMigrated(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated();
            context.Database.Migrate();

            SeedDatabase(serviceProvider);
        }
    }
}
