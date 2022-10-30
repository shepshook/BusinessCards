namespace BusinessCards.Api;

using BusinessCards.Domain;
using BusinessCards.Infrastructure;
using MediatR;

public class GetUserCardsQuery : IRequest<List<BusinessCard>>
{ }

public class GetUserCardsQueryHandler : IRequestHandler<GetUserCardsQuery, List<BusinessCard>>
{
    private readonly CardsRepository _repository;
    private readonly IUserAccessor _userAccessor;

    public GetUserCardsQueryHandler(CardsRepository repository, IUserAccessor userAccessor)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    public Task<List<BusinessCard>> Handle(GetUserCardsQuery query, CancellationToken ct)
    {
        var userId = _userAccessor.UserId;
        var cards = _repository.Filter(x => x.OwnerId == userId || x.SharedUsersIds.Contains(userId), ct);

        return cards;
    }
}
