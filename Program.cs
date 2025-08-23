using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;

namespace Training_Management_System_ITI_Project
{
  /// <summary>
  /// Main entry point for the Training Management System application.
  /// This ASP.NET Core MVC application provides comprehensive training management
  /// with full CRUD operations for Users, Courses, Sessions, and Grades.
  /// Implements the Repository Pattern for data access and comprehensive validation.
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

      // Configure ASP.NET Core Identity for authentication and authorization
      builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
      {
        // Password requirements
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.AllowedUserNameCharacters =
                      "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;

        // Sign-in settings
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
      })
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders();

      // Configure authentication cookie settings
      builder.Services.ConfigureApplicationCookie(options =>
      {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = true;
      });

      // Register Repository Pattern implementations for dependency injection
      // Each repository handles data access for a specific entity type
      builder.Services.AddScoped<IUserRepository, UserRepository>();
      builder.Services.AddScoped<ICourseRepository, CourseRepository>();
      builder.Services.AddScoped<ISessionRepository, SessionRepository>();
      builder.Services.AddScoped<IGradeRepository, GradeRepository>();

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

      // Enable authentication middleware - must be before authorization
      app.UseAuthentication();

      // Enable authorization middleware for role-based access control
      app.UseAuthorization();

      // Configure default route pattern
      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");

      await app.RunAsync();
    }

    /// <summary>
    /// Initializes the database and ensures all tables are created
    /// </summary>
    /// <param name="app">The web application instance</param>
    private static async Task InitializeDatabaseAsync(WebApplication app)
    {
      using var scope = app.Services.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      // Ensure database is created with all tables
      await context.Database.EnsureCreatedAsync();

      // Log successful database initialization
      var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
      logger.LogInformation("Database initialized successfully");
    }
  }
}
