namespace BusinessCards.Infrastructure;

using System.Linq.Expressions;
using BusinessCards.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class CardsRepository : ICardsRepository
{
    private readonly IMongoCollection<BusinessCardData> _collection;

    public CardsRepository(IOptions<DbSettings> dbSettings)
    {
        var settings = dbSettings?.Value ?? throw new ArgumentNullException(nameof(dbSettings));

        var mongoClient = new MongoClient(settings.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(settings.DatabaseName);
        _collection = mongoDb.GetCollection<BusinessCardData>(settings.CardsCollectionName);
    }

    public async Task<BusinessCard> Get(string id, CancellationToken ct)
    {
        var card = await _collection.Find(x => x.Id == id).SingleAsync(ct);
        return card.ToDomain();
    }

    public async Task<List<BusinessCard>> Filter(Expression<Func<BusinessCardData, bool>> predicate, CancellationToken ct)
    {
        var result = await _collection.Find(predicate).ToListAsync(ct);
        return result.Select(x => x.ToDomain()).ToList();
    }

    public Task Create(BusinessCard card, CancellationToken ct) =>
        _collection.InsertOneAsync(card.ToData(), cancellationToken: ct);

    public Task Update(string id, BusinessCard card, CancellationToken ct) =>
        _collection.ReplaceOneAsync(x => x.Id == id, card.ToData(), cancellationToken: ct);

    public Task Delete(string id, CancellationToken ct) =>
        _collection.DeleteOneAsync(x => x.Id == id, cancellationToken: ct);
}
