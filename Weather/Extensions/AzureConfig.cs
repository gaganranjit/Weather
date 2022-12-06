namespace Weather.Extensions
{
    public class AzureConfig
    {
        public string Instance { get; set; }

        public string TenantId { get; set; }
        public string ClientId { get; set; }

        /// <summary>
        /// Instanance/TennantId
        /// </summary>
        public string Authority { get { return $"{Instance}/{TenantId}"; } }
        /// <summary>
        /// Instanance/TennantId/oauth2/v2.0/authorize
        /// </summary>
        public string Authorize { get { return $"{Instance}/{TenantId}/oauth2/v2.0/authorize"; } }
        /// <summary>
        /// Instanance/TennantId/oauth2/v2.0/token
        /// </summary>
        public string TokenUrl { get { return $"{Authority}/oauth2/v2.0/token"; } }


        public string Audience { get { return $"api://{ClientId}"; } }

        public List<string> Permissions { get; set; }
        public List<string> Scopes { get; set; }
    }
}
