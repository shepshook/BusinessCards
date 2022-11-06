using BusinessCards.Domain;
using BusinessCards.Infrastructure.Auth;
using BusinessCards.Infrastructure.Database;
using MediatR;

namespace BusinessCards.Api;

public record CreateCardCommand(string Name, string Position, string Company, List<Contact> Contacts) : IRequest;

public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand>
{
    private readonly ICardsRepository _repository;
    private readonly IUserAccessor _userAccessor;

    public CreateCardCommandHandler(ICardsRepository repository, IUserAccessor userAccessor)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    public async Task<Unit> Handle(CreateCardCommand command, CancellationToken ct)
    {
        var card = new BusinessCard
        (
            _userAccessor.UserId,
            command.Name,
            command.Position,
            command.Company,
            command.Contacts.ToList()
        );

        await _repository.Create(card, ct);

        return Unit.Value;
    }
}
