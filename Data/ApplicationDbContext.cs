using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Data
{
  /// <summary>
  /// Entity Framework DbContext for the Training Management System.
  /// Defines the database schema and basic entity configurations.
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
    /// Database table for Course entities
    /// </summary>
    public DbSet<Course> Courses { get; set; }

    /// <summary>
    /// Database table for Session entities
    /// </summary>
    public DbSet<Session> Sessions { get; set; }

    /// <summary>
    /// Database table for User entities
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Database table for Grade entities
    /// </summary>
    public DbSet<Grade> Grades { get; set; }

    /// <summary>
    /// Configures entity relationships and basic database schema using Fluent API.
    /// This method is called by Entity Framework to build the database model.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure Course entity with basic properties
      modelBuilder.Entity<Course>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Category).IsRequired();
      });

      // Configure Session entity with basic properties
      modelBuilder.Entity<Session>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.StartDate).IsRequired();
        entity.Property(e => e.EndDate).IsRequired();
      });

      // Configure User entity with basic properties
      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Email).IsRequired();
        entity.Property(e => e.Role).IsRequired();
      });

      // Configure Grade entity with basic properties
      modelBuilder.Entity<Grade>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Value).IsRequired();
      });
    }
  }
}
