namespace BusinessCards.Infrastructure.Auth;

public class AuthOptions
{
    public const string Section = "Auth";

    public string Authority { get; set; }

    public string Audience { get; set; }
}
