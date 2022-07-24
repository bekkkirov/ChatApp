namespace ChatApp.Application.Common.Exceptions;

/// <summary>
/// Represents an identity exception.
/// </summary>
public class IdentityException : Exception
{
    /// <summary>
    /// Creates the new instance of the identity exception.
    /// </summary>
    public IdentityException()
    {
        
    }

    /// <summary>
    /// Creates the new instance of the identity exception with a specified message.
    /// </summary>
    /// <param name="message">Error message.</param>
    public IdentityException(string message) : base(message)
    {
        
    }
}