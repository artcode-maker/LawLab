using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LawLab.Models;
using LawLab.Infrastructure;
using LawLab.Repository;
using LawLab.Hubs;
using Microsoft.AspNetCore.Identity;

namespace LawLab
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGenericRepository<Student>, GenericRepository<Student>>();
            services.AddSingleton<IGenericRepository<Client>, GenericRepository<Client>>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".LawLab.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(240);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

            services.AddSignalR();
            services.AddCors();
            services.AddTransient<IPasswordValidator<AppUser>, LawLabPasswordValidator>();
            services.AddTransient<IUserValidator<AppUser>, LawLabUserValidator>();

            services.AddMvc();
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(conString);
            });

            services.AddIdentity<AppUser, IdentityRole>(opts => 
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters =
                    "∏ÈˆÛÍÂÌ„¯˘Áı˙Ù˚‚‡ÔÓÎ‰Ê˝ˇ˜ÒÏËÚ¸·˛®…÷” ≈Õ√ÿŸ«’‘€¬¿œ–ŒÀƒ∆›ﬂ◊—Ã»“¡ﬁ";
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseCors(policy =>
            {
                policy
                    .SetIsOriginAllowed(origin => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/startconsult");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();                
            });
            AppDbContext.CreateAdminAccountAndRoles(app.ApplicationServices, Configuration).Wait();
        }
    }
}
