using BusinessCards.Domain;
using BusinessCards.Infrastructure.Database;
using MediatR;

namespace BusinessCards.Api;

public record UpdateCardCommand(string Id, string Name, string Position, string Company, List<string> SharedUsersIds, List<Contact> Contacts) : IRequest;

public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand>
{
    private readonly ICardsRepository _repository;

    public UpdateCardCommandHandler(ICardsRepository repository) => _repository = repository;

    public async Task<Unit> Handle(UpdateCardCommand command, CancellationToken ct)
    {
        var card = await _repository.Get(command.Id, ct);
        
        card.UpdateName(command.Name);
        card.UpdatePosition(command.Position);
        card.UpdateCompany(command.Company);
        card.UpdateSharedUsersIds(command.SharedUsersIds);
        card.UpdateContacts(command.Contacts);

        await _repository.Update(command.Id, card, ct);

        return Unit.Value;
    }
}
