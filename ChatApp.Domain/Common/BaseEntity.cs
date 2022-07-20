namespace ChatApp.Domain.Common;

/// <summary>
/// Represent a base entity in this application.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the id for this entity.
    /// </summary>
    public int Id { get; set; }
}