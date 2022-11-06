using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace BusinessCards.Infrastructure.Auth;

public class JwtValidationKeysProvider : IKeysProvider
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(6);
    private readonly IMemoryCache _cache;
    private readonly HttpClient _http;

    public JwtValidationKeysProvider(IMemoryCache cache, HttpClient http)
    {
        _cache = cache;
        _http = http;
    }

    public Task<IEnumerable<SecurityKey>> GetKeys() =>
        _cache.GetOrCreateAsync<IEnumerable<SecurityKey>>("keys",
            async entry =>
            {
                var (keys, expiration) = await RenewKeys();
                entry.SetAbsoluteExpiration(expiration);

                return keys.Select(key =>
                    new RsaSecurityKey(
                        new X509Certificate2(key).GetRSAPublicKey()));
            });

    private async Task<(IEnumerable<byte[]> Keys, DateTimeOffset Expiration)> RenewKeys()
    {
        var response = await _http.GetAsync("https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com");

        var maxAge = response.Headers.CacheControl?.MaxAge;
        var expiration = DateTimeOffset.UtcNow.Add(maxAge ?? DefaultExpiration);

        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>() ?? new();
        var keys = result.Values.Select(x => Encoding.UTF8.GetBytes(x));

        return (keys, expiration);
    }
}
