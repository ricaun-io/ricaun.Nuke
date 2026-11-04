using Nuke.Common;
using ricaun.Nuke.IO;
using System;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// Provides helper functionality to exchange a GitHub Actions OIDC token for a NuGet API key token identifier.
    /// </summary>
    /// <remarks>Requires GitHub Actions OIDC support with workflow permissions that include `id-token:
    /// write`. Uses GitHub OIDC request environment variables and NuGet token service settings from environment
    /// variables when available.</remarks>
    public static class NugetActionTokenIdUtils
    {
        private static string _apiKey;

        /// <summary>
        /// Get NugetTokenId from GitHub OIDC token exchange for NuGet API key.
        /// </summary>
        /// <remarks>
        /// Based in the code: https://github.com/NuGet/login/blob/main/src/index.ts
        /// </remarks>
        public static string GetNugetTokenId(string nugetUsername)
        {
            if (!string.IsNullOrEmpty(_apiKey))
                return _apiKey;

            if (string.IsNullOrEmpty(nugetUsername))
            {
                throw new Exception("NuGet username is missing. Make sure you are using the username of the policy creator, not the policy owner.");
            }

            string nugetTokenServiceUrl = EnvironmentInfo.GetVariable("NUGET_TOKEN_SERVICE_URL") ?? "https://www.nuget.org/api/v2/token";
            string nugetAudience = EnvironmentInfo.GetVariable("NUGET_AUDIENCE") ?? "https://www.nuget.org";

            string oidcRequestToken = EnvironmentInfo.GetVariable("ACTIONS_ID_TOKEN_REQUEST_TOKEN");
            string oidcRequestUrl = EnvironmentInfo.GetVariable("ACTIONS_ID_TOKEN_REQUEST_URL");

            if (string.IsNullOrEmpty(oidcRequestToken) && string.IsNullOrEmpty(oidcRequestUrl))
            {
                throw new Exception(
                    "GitHub OIDC is not available. Ensure your workflow has:\n" +
                    "  permissions:\n" +
                    "    id-token: write\n" +
                    "    contents: read");
            }

            if (string.IsNullOrEmpty(oidcRequestToken))
            {
                throw new Exception("ACTIONS_ID_TOKEN_REQUEST_TOKEN is missing. Ensure your workflow has id-token: write.");
            }

            if (string.IsNullOrEmpty(oidcRequestUrl))
            {
                throw new Exception("ACTIONS_ID_TOKEN_REQUEST_URL is missing. Ensure your workflow has id-token: write.");
            }

            string tokenUrl = $"{oidcRequestUrl}&audience={Uri.EscapeDataString(nugetAudience)}";

            var result = HttpAuthTasks.HttpGet<ValueResponse>(tokenUrl, oidcRequestToken);
            var authorizationToken = result?.value;

            if (string.IsNullOrEmpty(authorizationToken))
            {
                throw new Exception("Failed to retrieve OIDC token from GitHub. Verify the audience and token service URL.");
            }

            var requestContent = new NugetTokenRequest
            {
                username = nugetUsername,
                tokenType = "ApiKey",
            };
            var nugetTokenResponse = HttpAuthTasks.HttpPost<NugetTokenResponse>(nugetTokenServiceUrl, requestContent, authorizationToken,
                headerConfigurator: headers =>
                {
                    headers.Add("User-Agent", $"ricaun/Nuke");
                });

            if (!string.IsNullOrEmpty(nugetTokenResponse?.error))
            {
                string errorMessage = $"Token exchange failed at {nugetTokenServiceUrl}. Make sure you are using the username of the policy creator, not the policy owner: {nugetTokenResponse?.error}";
                throw new Exception(errorMessage);
            }

            var apiKey = nugetTokenResponse?.apiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("Response did not contain 'apiKey'.");
            }

            Serilog.Log.Information("Successfully exchanged OIDC token for NuGet API key.");

            _apiKey = apiKey;
            return apiKey;
        }

        internal class ValueResponse
        {
            public string value { get; set; }
        }

        internal class NugetTokenRequest
        {
            public string username { get; set; }
            public string tokenType { get; set; }
        }

        internal class NugetTokenResponse
        {
            public string error { get; set; }
            public string tokenType { get; set; }
            public DateTime expires { get; set; }
            public string apiKey { get; set; }
        }
    }
}
