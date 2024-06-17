using Microsoft.AspNetCore.Identity;
using TerranForum.Domain.Models;
using TerranForum.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TerranForum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("TerranForum") ??
                throw new NullReferenceException("Connection string was null");

            builder.Services.AddRouting(options => 
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

            builder.Services.AddInfrastructure(connectionString, builder.Configuration);

            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options => 
                {
                    options.SignIn.RequireConfirmedEmail = false;
                    options.User.RequireUniqueEmail = true;
                    options.Lockout.MaxFailedAccessAttempts = 8;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TerranForumDbContext>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
