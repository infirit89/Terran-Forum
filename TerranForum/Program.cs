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

            builder.Services.AddDbContext<TerranForumDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedEmail = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TerranForumDbContext>();

            builder.Services.AddInfrastructure(connectionString, builder.Configuration);
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
                pattern: "{controller=forum}/{action=all}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
