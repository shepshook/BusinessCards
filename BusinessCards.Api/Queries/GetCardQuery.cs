using BusinessCards.Domain;
using BusinessCards.Infrastructure.Database;
using MediatR;

namespace BusinessCards.Api;

public class GetCardQuery : IRequest<BusinessCard>
{
    public string Id { get; }

    public GetCardQuery(string id)
    {
        Id = id;
    }
}

public class GetCardQueryHandler : IRequestHandler<GetCardQuery, BusinessCard>
{
    private readonly ICardsRepository _repository;

    public GetCardQueryHandler(ICardsRepository repository) => 
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public Task<BusinessCard> Handle(GetCardQuery query, CancellationToken ct) => 
        _repository.Get(query.Id, ct);
}
