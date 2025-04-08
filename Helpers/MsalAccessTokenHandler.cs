using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Security.Cryptography.X509Certificates;


namespace helpdesk_prove_request.Helpers;
public class MsalAccessTokenHandler
{
    /// <summary>
    /// Gets the access token for the client application using the client credentials flow.
    /// </summary>
    public static async Task<(string token, string error, string error_description)> GetAccessToken(IConfiguration configuration)
    {
        string tenantId = configuration["VerifiedID:TenantId"] ?? string.Empty;
        string clientId = configuration["VerifiedID:ClientId"] ?? string.Empty;
        string certificateThumbprint = configuration["VerifiedID:CertificateThumbprint"] ?? string.Empty;
        string[] scopes = { configuration["VerifiedID:scope"] ?? string.Empty };
        string authority = $"{configuration["VerifiedID:Authority"]}{tenantId}";

        // Check if the TenantId key is set in the appsettings.json file
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return (string.Empty, "500", "Missing the 'VerifiedID:TenantId' configuration for Entra ID. Please check the appsettings.json file.");
        }

        // Check if the ClientId key is set in the appsettings.json file
        if (string.IsNullOrWhiteSpace(clientId))
        {
            return (string.Empty, "500", "Missing the 'VerifiedID:ClientId' configuration for Entra ID. Please check the appsettings.json file.");
        }

        // Check if the VerifiedID:CertificateThumbprint key is set in the appsettings.json file
        if (string.IsNullOrWhiteSpace(certificateThumbprint))
        {
            return (string.Empty, "500", "Missing the 'VerifiedID:CertificateThumbprint' configuration for Entra ID. Please check the appsettings.json file.");
        }

        // Check if the VerifiedID:scope key is set in the appsettings.json file
        if (string.IsNullOrWhiteSpace(scopes[0]))
        {
            return (string.Empty, "500", "Missing the 'VerifiedID:scope' configuration for Entra ID. Please check the appsettings.json file.");
        }

        // Check if the VerifiedID:Authority key is set in the appsettings.json file
        if (string.IsNullOrWhiteSpace(authority))
        {
            return (string.Empty, "500", "Missing the 'VerifiedID:Authority' configuration for Entra ID. Please check the appsettings.json file.");
        }

        // Since we are using application permissions this will be a confidential client application
        X509Certificate2 certificate = ReadCertificate(certificateThumbprint);
        IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
              .WithCertificate(certificate)
              .WithAuthority(new Uri(authority))
              .Build();

        // Aquire a token for the client application using the client credentials flow
        AuthenticationResult? result = null;
        try
        {
            result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        }
        catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
        {
            return (string.Empty, "500", "Scope provided is not supported");
        }
        catch (MsalServiceException ex)
        {
            return (string.Empty, "500", "Something went wrong getting an access token for the client API:" + ex.Message);
        }

        return (result.AccessToken, string.Empty, string.Empty);
    }

    /// <summary>
    /// Reads the certificate from the store using the thumbprint provided in the appsettings.json file.
    /// </summary>
    private static X509Certificate2 ReadCertificate(string certificateThumbprint)
    {
        if (string.IsNullOrWhiteSpace(certificateThumbprint))
        {
            throw new ArgumentException("certificateThumbprint should not be empty. Please set the certificateThumbprint setting in the appsettings.json", "certificateThumbprint");
        }
        CertificateDescription certificateDescription = CertificateDescription.FromStoreWithThumbprint(
             certificateThumbprint,
             StoreLocation.CurrentUser,
             StoreName.My);

        DefaultCertificateLoader defaultCertificateLoader = new DefaultCertificateLoader();
        defaultCertificateLoader.LoadIfNeeded(certificateDescription);

        if (certificateDescription.Certificate == null)
        {
            throw new Exception("Cannot find the certificate.");
        }

        return certificateDescription.Certificate;
    }
}

