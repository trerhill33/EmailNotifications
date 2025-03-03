namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Interface for accessing the service provider
/// </summary>
public interface IServiceProviderAccessor
{
    /// <summary>
    /// Gets the service provider
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}