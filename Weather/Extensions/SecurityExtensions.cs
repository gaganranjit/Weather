using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Weather.Extensions
{
    public static class SecurityExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"{configuration["AzureAd:Instance"]}{configuration["AzureAd:TenantId"]}";
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["AzureAd:Instance"],
                    ValidAudiences = new List<string> { configuration["AzureAd:Audience"] }
                };
            });
            return services;
        }


        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var permissionSection = configuration.GetSection("AzureAd:Permissions");
            var scopeSection = configuration.GetSection("AzureAd:Scopes");
            var permissions = permissionSection.Get<string[]>();
            var scopes = scopeSection.Get<string[]>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(configuration["AzureAd:PolicyName"], policyBuilder =>
                 policyBuilder
                 .RequireAssertion(handler =>
                 {
                     var roleClaims = handler.User.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();
                     var scopeClaims = handler.User.FindAll("http://schemas.microsoft.com/identity/claims/scope").ToList();

                     bool hasRoleClaim = roleClaims.Any(r => permissions.Contains(r.Value));
                     bool hasScopeClaim = scopeClaims.Any() && scopes.Any(s => scopeClaims.FirstOrDefault().Value.Contains(s));

                     return hasRoleClaim || hasScopeClaim;
                 }));
            });

            return services;
        }
    }
}
