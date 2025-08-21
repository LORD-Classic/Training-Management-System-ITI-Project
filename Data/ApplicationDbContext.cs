using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Data
{
  /// <summary>
  /// Entity Framework DbContext for the Training Management System with ASP.NET Core Identity integration.
  /// Defines the database schema, relationships, and constraints for all entities.
  /// Implements business rules through database constraints and indexes.
  /// Extends IdentityDbContext to provide authentication and authorization capabilities.
  /// </summary>
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    /// <summary>
    /// Initializes a new instance of the ApplicationDbContext
    /// </summary>
    /// <param name="options">Database context options containing connection string and provider info</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSet properties represent database tables for each entity type

    /// <summary>
    /// Database table for Course entities
    /// </summary>
    public DbSet<Course> Courses { get; set; }

    /// <summary>
    /// Database table for Session entities
    /// </summary>
    public DbSet<Session> Sessions { get; set; }

    /// <summary>
    /// Database table for legacy User entities (for backward compatibility)
    /// Note: ApplicationUser (Identity) is used for authentication, this is for legacy support
    /// </summary>
    public DbSet<User> LegacyUsers { get; set; }

    /// <summary>
    /// Database table for Grade entities
    /// </summary>
    public DbSet<Grade> Grades { get; set; }

    /// <summary>
    /// Configures entity relationships, constraints, and database schema using Fluent API.
    /// This method is called by Entity Framework to build the database model.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure Course entity with business rules
      modelBuilder.Entity<Course>(entity =>
      {
        entity.HasKey(e => e.Id);

        // Enforce unique course names business rule
        entity.HasIndex(e => e.Name).IsUnique();
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Category).IsRequired();

        // Configure optional relationship with Instructor
        // If instructor is deleted, course instructor is set to null (not deleted)
        entity.HasOne(c => c.Instructor)
                    .WithMany(u => u.CoursesAsInstructor)
                    .HasForeignKey(c => c.InstructorId)
                    .OnDelete(DeleteBehavior.SetNull);
      });

      // Configure Session entity with date validation constraints
      modelBuilder.Entity<Session>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.StartDate).IsRequired();
        entity.Property(e => e.EndDate).IsRequired();

        // Configure required relationship with Course
        // If course is deleted, all its sessions are also deleted (cascade)
        entity.HasOne(s => s.Course)
                    .WithMany(c => c.Sessions)
                    .HasForeignKey(s => s.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
      });

      // Configure User entity with email uniqueness constraint
      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Email).IsRequired();
        entity.Property(e => e.Role).IsRequired();

        // Enforce unique email addresses business rule
        entity.HasIndex(e => e.Email).IsUnique();
      });

      // Configure Grade entity with business constraints
      modelBuilder.Entity<Grade>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Value).IsRequired();

        // Configure required relationship with Session
        // If session is deleted, all grades for that session are deleted
        entity.HasOne(g => g.Session)
                    .WithMany(s => s.Grades)
                    .HasForeignKey(g => g.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);

        // Configure required relationship with Trainee
        // If trainee is deleted, all their grades are deleted
        entity.HasOne(g => g.Trainee)
                    .WithMany(u => u.Grades)
                    .HasForeignKey(g => g.TraineeId)
                    .OnDelete(DeleteBehavior.Cascade);

        // Business rule: One grade per trainee per session (prevent duplicate grades)
        entity.HasIndex(e => new { e.SessionId, e.TraineeId }).IsUnique();
      });
    }
  }
}
