using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Template.QrCode.Api.Helpers
{
    public interface ICertificateService
    {
        X509Certificate2 GetCertificate();
    }

    public class CertificateService : ICertificateService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CertificateService> _logger;
        private static X509Certificate2 _cachedCertificate;
        private static readonly object _lock = new object();

        public CertificateService(IConfiguration configuration, ILogger<CertificateService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public X509Certificate2 GetCertificate()
        {
            // Return cached certificate if available
            if (_cachedCertificate != null)
            {
                return _cachedCertificate;
            }

            // Thread-safe certificate loading
            lock (_lock)
            {
                // Double-check pattern
                if (_cachedCertificate != null)
                {
                    return _cachedCertificate;
                }

                try
                {
                    string certificateThumbprint = _configuration["HashCertificateThumbprint"];
                    string certificateSubjectName = _configuration["HashCertificateSubjectName"];

                    X509Certificate2 cert = null;

                    // Open the Windows Certificate Store
                    using (X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
                    {
                        store.Open(OpenFlags.ReadOnly);

                        // Find certificate by thumbprint (preferred method)
                        if (!string.IsNullOrEmpty(certificateThumbprint))
                        {
                            X509Certificate2Collection certCollection = store.Certificates.Find(
                                X509FindType.FindByThumbprint,
                                certificateThumbprint,
                                false);

                            if (certCollection.Count > 0)
                            {
                                cert = certCollection[0];
                            }
                        }
                        // Alternative: Find by subject name
                        else if (!string.IsNullOrEmpty(certificateSubjectName))
                        {
                            X509Certificate2Collection certCollection = store.Certificates.Find(
                                X509FindType.FindBySubjectName,
                                certificateSubjectName,
                                false);

                            if (certCollection.Count > 0)
                            {
                                cert = certCollection[0];
                            }
                        }

                        store.Close();
                    }

                    if (cert == null)
                    {
                        _logger.LogError("Certificate not found in Windows Certificate Store");
                        throw new InvalidOperationException("Certificate not found in Windows Certificate Store");
                    }

                    if (!cert.HasPrivateKey)
                    {
                        _logger.LogError("Certificate does not have a private key");
                        throw new InvalidOperationException("Certificate does not have a private key");
                    }

                    _logger.LogInformation("Certificate loaded successfully from store");

                    // Cache the certificate
                    _cachedCertificate = cert;

                    return _cachedCertificate;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading certificate from Windows Certificate Store");
                    throw;
                }
            }
        }
    }
}
