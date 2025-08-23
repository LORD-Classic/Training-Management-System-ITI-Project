using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Data
{
  /// <summary>
  /// Entity Framework DbContext for the Training Management System.
  /// Defines the database schema and entity configurations for user management.
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
    /// Configures entity relationships and database schema using Fluent API.
    /// This method is called by Entity Framework to build the database model.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure User entity with business rules
      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Email).IsRequired();
        entity.Property(e => e.Role).IsRequired();

        // Enforce unique email addresses business rule
        entity.HasIndex(e => e.Email).IsUnique();
      });
    }
  }
}
