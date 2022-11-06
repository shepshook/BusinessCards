using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace BusinessCards.Infrastructure.Auth;

public class FirebaseJwtValidationKeysProvider : IKeyProvider
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(6);
    private readonly IMemoryCache _cache;
    private readonly HttpClient _http;

    public FirebaseJwtValidationKeysProvider(IMemoryCache cache, HttpClient http)
    {
        _cache = cache;
        _http = http;
    }

    public Task<SecurityKey> GetKey(string keyId) =>
        _cache.GetOrCreateAsync<SecurityKey>(keyId,
            async entry =>
            {
                var (key, expiration) = await RenewKey(keyId);
                entry.SetAbsoluteExpiration(expiration);

                return key;
            });

    private async Task<(SecurityKey Key, DateTimeOffset Expiration)> RenewKey(string keyId)
    {
        var response = await _http.GetAsync("https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com");

        var maxAge = response.Headers.CacheControl?.MaxAge;
        var expiration = DateTimeOffset.UtcNow.Add(maxAge ?? DefaultExpiration);

        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>() ?? new();
        var key = result.GetValueOrDefault(keyId) 
            ?? throw new ArgumentException("Cannot locate a matching public key with matching KeyId (kid)", nameof(keyId));

        return (BuildKey(keyId, key), expiration);
    }

    private SecurityKey BuildKey(string keyId, string keyValue)
    {
        var keyBytes = Encoding.UTF8.GetBytes(keyValue);

        var securityKey = new RsaSecurityKey(
            new X509Certificate2(keyBytes).GetRSAPublicKey());
        
        securityKey.KeyId = keyId;
        return securityKey;
    }
}
