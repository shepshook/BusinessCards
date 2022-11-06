using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace BusinessCards.Infrastructure.Auth;

public class ConfigureValidationKeysJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly IKeysProvider _keysProvider;

    public ConfigureValidationKeysJwtBearerOptions(IKeysProvider keysProvider) => _keysProvider = keysProvider;

    public async void PostConfigure(string name, JwtBearerOptions options) => 
        options.TokenValidationParameters.IssuerSigningKeys = await _keysProvider.GetKeys();
}
