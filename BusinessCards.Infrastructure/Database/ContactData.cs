using BusinessCards.Domain;

namespace BusinessCards.Infrastructure.Database;

public class ContactData
{
    public ContactType ContactType { get; set; }

    public string Value { get; set; }
}
