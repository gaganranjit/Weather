using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Weather.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {

            var apiConfig = configuration.GetSection("AzureAd").Get<AzureConfig>();

            services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(apiConfig.Authorize, UriKind.Absolute),
                            TokenUrl = new Uri(apiConfig.TokenUrl, UriKind.Absolute),
                            Scopes = new Dictionary<string, string>
                            {
                                { $"{apiConfig.Audience}/Weather.ReadWrite", "Weather Permission" },
                            }
                        },
                    }
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                },
                                Scheme = "oauth2",
                                Name = "oauth2",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                });

            });

            return services;
        }
    }
}
