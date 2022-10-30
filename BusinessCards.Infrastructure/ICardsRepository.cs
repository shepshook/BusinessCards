namespace BusinessCards.Infrastructure;

using System.Linq.Expressions;
using BusinessCards.Domain;

public interface ICardsRepository
{
    Task Create(BusinessCard card, CancellationToken ct);
    Task Delete(string id, CancellationToken ct);
    Task<List<BusinessCard>> Filter(Expression<Func<BusinessCardData, bool>> predicate, CancellationToken ct);
    Task<BusinessCard> Get(string id, CancellationToken ct);
    Task Update(string id, BusinessCard card, CancellationToken ct);
}
