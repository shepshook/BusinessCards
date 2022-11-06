using BusinessCards.Domain;
using BusinessCards.Infrastructure.Auth;
using BusinessCards.Infrastructure.Database;
using MediatR;

namespace BusinessCards.Api;

public class GetUserCardsQuery : IRequest<List<BusinessCard>>
{ }

public class GetUserCardsQueryHandler : IRequestHandler<GetUserCardsQuery, List<BusinessCard>>
{
    private readonly ICardsRepository _repository;
    private readonly IUserAccessor _userAccessor;

    public GetUserCardsQueryHandler(ICardsRepository repository, IUserAccessor userAccessor)
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
