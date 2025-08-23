using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;

namespace Training_Management_System_ITI_Project
{
  /// <summary>
  /// Main entry point for the Training Management System application.
  /// This ASP.NET Core MVC application manages sessions using the Repository Pattern for data access.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Application entry point - configures services and middleware pipeline
    /// </summary>
    /// <param name="args">Command line arguments</param>
    public static async Task Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Configure MVC services for controllers and views
      builder.Services.AddControllersWithViews();

      // Configure Entity Framework with SQL Server database
      // Connection string is retrieved from appsettings.json
      builder.Services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      // Register Repository Pattern implementations for dependency injection
      builder.Services.AddScoped<IUserRepository, UserRepository>();
      builder.Services.AddScoped<ICourseRepository, CourseRepository>();
      builder.Services.AddScoped<ISessionRepository, SessionRepository>();

      var app = builder.Build();

      // Initialize database
      await InitializeDatabaseAsync(app);

      // Configure the HTTP request pipeline for different environments
      if (!app.Environment.IsDevelopment())
      {
        // Production error handling - redirect to friendly error page
        app.UseExceptionHandler("/Home/Error");
        // HTTP Strict Transport Security (HSTS) for enhanced security
        app.UseHsts();
      }

      // Enforce HTTPS redirection for secure communication
      app.UseHttpsRedirection();

      // Enable serving static files (CSS, JS, images) from wwwroot
      app.UseStaticFiles();

      // Enable routing to match URLs to controllers/actions
      app.UseRouting();

      // Configure default route pattern
      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");

      await app.RunAsync();
    }

    /// <summary>
    /// Initializes the database
    /// </summary>
    /// <param name="app">The web application instance</param>
    private static async Task InitializeDatabaseAsync(WebApplication app)
    {
      using var scope = app.Services.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      // Ensure database is created
      await context.Database.EnsureCreatedAsync();
    }
  }
}
