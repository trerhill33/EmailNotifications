using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CertificateConverter;

/// <summary>
/// Utility program to convert certificates to base64 strings for embedding in code
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Certificate to Base64 Converter");
        Console.WriteLine("==============================");
        
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: CertificateConverter <path-to-certificate>");
            Console.WriteLine("Example: CertificateConverter C:\\path\\to\\certificate.cer");
            return;
        }

        string certificatePath = args[0];
        
        if (!File.Exists(certificatePath))
        {
            Console.WriteLine($"Error: Certificate file not found at {certificatePath}");
            return;
        }

        try
        {
            // Read the certificate file
            byte[] certificateBytes = File.ReadAllBytes(certificatePath);
            
            // Convert to base64 string
            string base64Data = Convert.ToBase64String(certificateBytes);
            
            // Output the base64 string
            Console.WriteLine("Certificate Base64 Data:");
            Console.WriteLine(base64Data);
            
            // Write the base64 string to a file as well
            string outputPath = Path.Combine(
                Path.GetDirectoryName(certificatePath),
                Path.GetFileNameWithoutExtension(certificatePath) + ".base64.txt");
            
            File.WriteAllText(outputPath, base64Data);
            Console.WriteLine($"Base64 data also saved to: {outputPath}");
            
            // Verify by loading the certificate
            try
            {
                var certificate = new X509Certificate2(certificateBytes);
                Console.WriteLine("\nCertificate Successfully Loaded");
                Console.WriteLine($"Subject: {certificate.Subject}");
                Console.WriteLine($"Issuer: {certificate.Issuer}");
                Console.WriteLine($"Valid From: {certificate.NotBefore}");
                Console.WriteLine($"Valid To: {certificate.NotAfter}");
                Console.WriteLine($"Thumbprint: {certificate.Thumbprint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nWarning: Certificate loaded but may not be usable: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting certificate: {ex.Message}");
        }
    }
} 