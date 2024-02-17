namespace Cohors.Exceptions;

/// <summary>
/// Exception that is thrown when a required configuration property is missing.
/// </summary>
/// <param name="property">The name of the missing configuration property.</param>
public class ConfigurationMissingException : Exception
{
    private const string DefaultErrorMsg = "Configuration property is missing: {0}";

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </summary>
    /// <param name="property">The name of the missing configuration property.</param>
    public ConfigurationMissingException(string property) 
        : base(string.Format(DefaultErrorMsg, property))
    {
    }
}