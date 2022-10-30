using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessCards.Infrastructure;

public class BusinessCardData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string OwnerId { get; set; }

    public List<string> SharedUsersIds { get; set; }

    public string Name { get; set; }

    public string Position { get; set; }

    public string Company { get; set; }

    public List<ContactData> Contacts { get; set; }
}
