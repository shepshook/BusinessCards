namespace BusinessCards.Api;

public interface IUserAccessor
{
    string UserId { get; }
}

public class UserAccessor : IUserAccessor
{
    public string UserId => "debug-user";
}
