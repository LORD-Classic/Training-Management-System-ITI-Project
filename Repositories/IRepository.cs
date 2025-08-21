namespace Training_Management_System_ITI_Project.Repositories
{
  /// <summary>
  /// Generic repository interface defining common data access operations.
  /// Implements the Repository Pattern to abstract data access logic from business logic.
  /// All entity-specific repositories inherit from this base interface for consistency.
  /// </summary>
  /// <typeparam name="T">The entity type this repository manages</typeparam>
  public interface IRepository<T> where T : class
  {
    /// <summary>
    /// Retrieves all entities of type T from the database
    /// </summary>
    /// <returns>A collection of all entities</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves a single entity by its primary key
    /// </summary>
    /// <param name="id">The primary key value to search for</param>
    /// <returns>The entity if found, null otherwise</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entity to the database
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>The added entity with any generated values (like Id)</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity in the database
    /// </summary>
    /// <param name="entity">The entity with updated values</param>
    /// <returns>The updated entity</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity from the database by its primary key
    /// </summary>
    /// <param name="id">The primary key of the entity to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Checks if an entity with the specified primary key exists
    /// </summary>
    /// <param name="id">The primary key to check for</param>
    /// <returns>True if the entity exists, false otherwise</returns>
    Task<bool> ExistsAsync(int id);
  }
}
