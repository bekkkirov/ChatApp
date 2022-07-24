using System.Runtime.Serialization;

namespace ChatApp.Application.Common.Exceptions;

/// <summary>
/// Represents a validation exception.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Creates the new instance of the validation exception.
    /// </summary>
    public ValidationException()
    {
    }

    /// <summary>
    /// Creates the new instance of the validation exception with specified message.
    /// </summary>
    /// <param name="message">Error message.</param>
    public ValidationException(string message) : base(message)
    {
    }
}