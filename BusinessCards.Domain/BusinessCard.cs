namespace BusinessCards.Domain;

public class BusinessCard
{
    private List<string> _sharedUsersIds = new List<string>();
    private List<Contact> _contacts;

    public string? Id { get; private set; }

    public string OwnerId { get; private set; }

    public string Name { get; private set; }

    public string Position { get; private set; }

    public string Company { get; private set; }

    public IReadOnlyCollection<string> SharedUsersIds => _sharedUsersIds.AsReadOnly();

    public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();

    public BusinessCard(string ownerId, string name, string position, string company, List<Contact> contacts)
    {
        OwnerId = ownerId;
        Name = name;
        Position = position;
        Company = company;
        _contacts = contacts;
    }

    public BusinessCard(string id, string ownerId, string name, string position, string company, List<string> sharedUsersIds, List<Contact> contacts)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        Position = position;
        Company = company;
        _sharedUsersIds = sharedUsersIds ?? new List<string>();
        _contacts = contacts ?? new List<Contact>();
    }

    public void UpdateName(string name) => Name = name;

    public void UpdatePosition(string position) => Position = position;

    public void UpdateCompany(string company) => Company = company;

    public void UpdateSharedUsersIds(IEnumerable<string> sharedUsersIds) => _sharedUsersIds = sharedUsersIds.ToList();

    public void UpdateContacts(IEnumerable<Contact> contacts) => _contacts = contacts.ToList();

    public void ShareWithUser(string userId) => _sharedUsersIds.Add(userId);
}

public record Contact(ContactType ContactType, string Value);
