using Microsoft.IdentityModel.Tokens;

namespace BusinessCards.Infrastructure.Auth;

public interface IKeysProvider
{
    Task<IEnumerable<SecurityKey>> GetKeys();
}
