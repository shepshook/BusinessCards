using BusinessCards.Domain;
using BusinessCards.Infrastructure.Auth;
using BusinessCards.Infrastructure.Database;
using MediatR;

namespace BusinessCards.Api;

public record CreateCardCommand(string Name, string Position, string Company, List<Contact> Contacts) : IRequest<string>;

public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, string>
{
    private readonly ICardsRepository _repository;
    private readonly IUserAccessor _userAccessor;

    public CreateCardCommandHandler(ICardsRepository repository, IUserAccessor userAccessor)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    public async Task<string> Handle(CreateCardCommand command, CancellationToken ct)
    {
        var card = new BusinessCard
        (
            _userAccessor.UserId,
            command.Name,
            command.Position,
            command.Company,
            command.Contacts.ToList()
        );

        var id = await _repository.Create(card, ct);

        return id;
    }
}
