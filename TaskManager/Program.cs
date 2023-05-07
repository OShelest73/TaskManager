using Microsoft.Extensions.Configuration;
using TaskManager.Data;

namespace TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();
            // Add services to the container.
            builder.Services.AddControllersWithViews();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "taskManagement",
                pattern: "/{controller}/{workspace}/{action}/{id}");
            app.MapControllerRoute(
                name: "create",
                pattern: "/{controller}/{workspace}/{action}");
            app.MapControllerRoute(
                name: "userManagement",
                pattern: "{controller=Users}/{action=Index}/{id}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Index}/{id?}");
            
            //TODO раскоментить если добавишь инициализатор
            //AddDbInitializer.Seed(app);

            app.Run();
        }
    }
}