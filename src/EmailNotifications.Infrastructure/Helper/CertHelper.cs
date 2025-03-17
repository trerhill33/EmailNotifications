using System.Security.Cryptography.X509Certificates;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Helper;

public interface ICertHelper
{  
    X509Certificate2Collection GetIntermediateCertificatesAsync(string? secretId, CancellationToken cancellationToken = default);
}

public class CertHelper(IAmazonSecretsManager secretsManager, ILogger<CertHelper> logger) : ICertHelper
{
    private readonly IAmazonSecretsManager _secretsManager = secretsManager ?? throw new ArgumentNullException(nameof(secretsManager));
    private readonly ILogger<CertHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public X509Certificate2Collection GetIntermediateCertificatesAsync(string? secretId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving intermediate certificate from Secrets Manager with secret ID: {SecretId}", secretId);
            var pemString = GetSecretStringAsync(secretId, cancellationToken);
            var certBytes = ParsePemToBytes(pemString);

            var certificates = new X509Certificate2Collection();
            certificates.Import(certBytes);

            _logger.LogInformation("Successfully retrieved and parsed intermediate certificate from {SecretId}", secretId);
            return certificates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve or parse intermediate certificate from Secrets Manager with secret ID: {SecretId}", secretId);
            throw new CertificateException($"Failed to retrieve or parse certificate from {secretId}", ex);
        }
    }

    private string GetSecretStringAsync(string? secretId, CancellationToken cancellationToken)
    {
        var request = new GetSecretValueRequest { SecretId = secretId };
        var response = (_secretsManager.GetSecretValueAsync(request, cancellationToken).GetAwaiter().GetResult());

        if (!string.IsNullOrEmpty(response.SecretString))
        {
            return response.SecretString;
        }

        throw new CertificateException($"Secret {secretId} does not contain a string value.");
    }

    private static byte[] ParsePemToBytes(string pemString)
    {
        try
        {
            // Remove PEM headers, footers, and whitespace
            var base64 = pemString
                .Replace("-----BEGIN CERTIFICATE-----", "")
                .Replace("-----END CERTIFICATE-----", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            return Convert.FromBase64String(base64);
        }
        catch (Exception ex)
        {
            throw new CertificateException("Failed to parse PEM certificate string", ex);
        }
    }
}

public class CertificateException : Exception
{
    public CertificateException(string message) : base(message) { }
    public CertificateException(string message, Exception innerException) : base(message, innerException) { }
}