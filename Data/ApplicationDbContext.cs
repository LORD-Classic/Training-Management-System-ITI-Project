using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Data
{
  /// <summary>
  /// Entity Framework DbContext for the Training Management System.
  /// Defines the complete database schema with comprehensive entity configurations,
  /// relationships, constraints, and business rules for all entities.
  /// </summary>
  public class ApplicationDbContext : DbContext
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
    /// Database table for User entities
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Database table for Course entities
    /// </summary>
    public DbSet<Course> Courses { get; set; }

    /// <summary>
    /// Database table for Session entities
    /// </summary>
    public DbSet<Session> Sessions { get; set; }

    /// <summary>
    /// Database table for Grade entities
    /// </summary>
    public DbSet<Grade> Grades { get; set; }

    /// <summary>
    /// Configures entity relationships, constraints, and database schema using Fluent API.
    /// This method is called by Entity Framework to build the database model.
    /// Implements comprehensive business rules and data integrity constraints.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure User entity with comprehensive business rules
      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(e => e.Id);
        
        // Property configurations
        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(50);
        
        entity.Property(e => e.Email)
              .IsRequired()
              .HasMaxLength(100);
        
        entity.Property(e => e.Role)
              .IsRequired();
        
        entity.Property(e => e.CreatedAt)
              .IsRequired();
        
        entity.Property(e => e.UpdatedAt)
              .IsRequired();

        // Enforce unique email addresses business rule
        entity.HasIndex(e => e.Email).IsUnique();

        // Configure relationships
        entity.HasMany(u => u.CoursesAsInstructor)
              .WithOne(c => c.Instructor)
              .HasForeignKey(c => c.InstructorId)
              .OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(u => u.Grades)
              .WithOne(g => g.Trainee)
              .HasForeignKey(g => g.TraineeId)
              .OnDelete(DeleteBehavior.Cascade);
      });

      // Configure Course entity with comprehensive business rules
      modelBuilder.Entity<Course>(entity =>
      {
        entity.HasKey(e => e.Id);
        
        // Property configurations
        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(50);
        
        entity.Property(e => e.Category)
              .IsRequired()
              .HasMaxLength(30);
        
        entity.Property(e => e.Description)
              .HasMaxLength(500);
        
        entity.Property(e => e.DurationHours)
              .HasDefaultValue(null);
        
        entity.Property(e => e.MaxTrainees)
              .HasDefaultValue(null);
        
        entity.Property(e => e.CreatedAt)
              .IsRequired();
        
        entity.Property(e => e.UpdatedAt)
              .IsRequired();

        // Enforce unique course names business rule
        entity.HasIndex(e => e.Name).IsUnique();

        // Configure relationships
        entity.HasOne(c => c.Instructor)
              .WithMany(u => u.CoursesAsInstructor)
              .HasForeignKey(c => c.InstructorId)
              .OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(c => c.Sessions)
              .WithOne(s => s.Course)
              .HasForeignKey(s => s.CourseId)
              .OnDelete(DeleteBehavior.Cascade);
      });

      // Configure Session entity with comprehensive business rules
      modelBuilder.Entity<Session>(entity =>
      {
        entity.HasKey(e => e.Id);
        
        // Property configurations
        entity.Property(e => e.CourseId)
              .IsRequired();
        
        entity.Property(e => e.StartDate)
              .IsRequired();
        
        entity.Property(e => e.EndDate)
              .IsRequired();
        
        entity.Property(e => e.Title)
              .HasMaxLength(100);
        
        entity.Property(e => e.Description)
              .HasMaxLength(500);
        
        entity.Property(e => e.Location)
              .HasMaxLength(200);
        
        entity.Property(e => e.CreatedAt)
              .IsRequired();
        
        entity.Property(e => e.UpdatedAt)
              .IsRequired();

        // Configure relationships
        entity.HasOne(s => s.Course)
              .WithMany(c => c.Sessions)
              .HasForeignKey(s => s.CourseId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(s => s.Grades)
              .WithOne(g => g.Session)
              .HasForeignKey(g => g.SessionId)
              .OnDelete(DeleteBehavior.Cascade);
      });

      // Configure Grade entity with comprehensive business rules
      modelBuilder.Entity<Grade>(entity =>
      {
        entity.HasKey(e => e.Id);
        
        // Property configurations
        entity.Property(e => e.SessionId)
              .IsRequired();
        
        entity.Property(e => e.TraineeId)
              .IsRequired();
        
        entity.Property(e => e.Value)
              .IsRequired();
        
        entity.Property(e => e.Comments)
              .HasMaxLength(1000);
        
        entity.Property(e => e.GradeDate)
              .IsRequired();
        
        entity.Property(e => e.UpdatedAt)
              .IsRequired();

        // Configure relationships
        entity.HasOne(g => g.Session)
              .WithMany(s => s.Grades)
              .HasForeignKey(g => g.SessionId)
              .OnDelete(DeleteBehavior.Cascade);

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
