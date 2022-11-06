using BusinessCards.Infrastructure.Auth;
using BusinessCards.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusinessCards.Api;

public static partial class StartupHelpers
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        var authOptions = new AuthOptions();
        builder.Configuration.GetSection(AuthOptions.Section).Bind(authOptions);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMemoryCache();
        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<ICardsRepository, CardsRepository>();
        builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(DbSettings.Section));

        builder.Services.AddHttpClient<FirebaseJwtValidationKeysProvider>();
        builder.Services.AddSingleton<IKeyProvider, FirebaseJwtValidationKeysProvider>();
        builder.Services.AddScoped<IUserAccessor, UserAccessor>();
        builder.Services
            .AddAuthorization()
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                var serviceProvider = builder.Services.BuildServiceProvider();

                opt.TokenValidationParameters = new TokenValidationParameters
                {
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
    }
}
