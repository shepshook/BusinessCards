namespace BusinessCards.Infrastructure.Auth;

public interface IUserAccessor
{
    string UserId { get; }

    string Name { get; }
}
