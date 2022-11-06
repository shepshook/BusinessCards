using BusinessCards.Infrastructure.Auth;
using BusinessCards.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BusinessCards.Api;

public static partial class StartupHelpers
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerSupport();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMemoryCache();
        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<ICardsRepository, CardsRepository>();
        builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(DbSettings.Section));

        builder.Services.AddJwtAuthentication(builder.Configuration);
    }

    private static IServiceCollection AddSwaggerSupport(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Put your bearer token here",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = new AuthOptions();
        configuration.GetSection(AuthOptions.Section).Bind(authOptions);

        services.AddHttpClient<FirebaseJwtValidationKeysProvider>();
        services.AddSingleton<IKeyProvider, FirebaseJwtValidationKeysProvider>();
        services.AddScoped<IUserAccessor, UserAccessor>();

        services
            .AddAuthorization()
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                var serviceProvider = services.BuildServiceProvider();

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKeyResolver = (_, _, keyId, _) =>
                    {
                        var keysProvider = serviceProvider.GetRequiredService<IKeyProvider>();
                        return new List<SecurityKey> 
                        {
                            keysProvider.GetKey(keyId).GetAwaiter().GetResult()
                        };
                    }
                };
                
                opt.Authority = authOptions.Authority;
                opt.Audience = authOptions.Audience;
                opt.MapInboundClaims = false;
            });

        return services;
    }

}
