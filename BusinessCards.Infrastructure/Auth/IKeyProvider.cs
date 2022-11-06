using Microsoft.IdentityModel.Tokens;

namespace BusinessCards.Infrastructure.Auth;

public interface IKeyProvider
{
    Task<SecurityKey> GetKey(string keyId);
}
