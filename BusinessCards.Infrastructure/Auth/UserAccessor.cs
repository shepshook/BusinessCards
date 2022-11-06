using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BusinessCards.Infrastructure.Auth;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserAccessor(IHttpContextAccessor context) => _contextAccessor = context;

    public string UserId => Identity.FindFirst("sub")?.Value ?? "unknown";

    public string Name => Identity.FindFirst("name")?.Value ?? "unknown";

    protected ClaimsPrincipal Identity => _contextAccessor.HttpContext!.User;
}
