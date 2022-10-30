namespace BusinessCards.Infrastructure;

using BusinessCards.Domain;
using MongoDB.Driver;

public static class DomainDataMapping
{
    public static BusinessCard ToDomain(this BusinessCardData from) =>
        new BusinessCard
        (
            from.Id,
            from.OwnerId,
            from.Name,
            from.Position,
            from.Company,
            from.SharedUsersIds,
            from.Contacts.Select(x => x.ToDomain()).ToList()
        );

    public static Contact ToDomain(this ContactData from) =>
        new Contact(from.ContactType, from.Value);

    public static BusinessCardData ToData(this BusinessCard from) =>
        new BusinessCardData
        {
            Id = from.Id,
            OwnerId = from.OwnerId,
            Name = from.Name,
            Position = from.Position,
            Company = from.Company,
            SharedUsersIds = from.SharedUsersIds.ToList(),
            Contacts = from.Contacts.Select(x => x.ToData()).ToList()
        };

    public static ContactData ToData(this Contact from) =>
        new ContactData
        {
            ContactType = from.ContactType,
            Value = from.Value
        };
}
