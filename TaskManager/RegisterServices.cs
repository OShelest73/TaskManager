﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Services;

namespace TaskManager;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Users/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });
        builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
        builder.Services.AddScoped<IUsersService, UsersService>();
        builder.Services.AddScoped<ICategoriesService, CategoriesService>();
        builder.Services.AddScoped<IWorkspacesService, WorkspaceService>();
        builder.Services.AddScoped<ITasksService, TasksService>();
        builder.Services.AddScoped<IStatusesService, StatusesService>();
        builder.Services.AddScoped<INotificationsService, NotificationsService>();
    }
}
