using BusinessCards.Infrastructure.Database;
using MediatR;

namespace BusinessCards.Api;

public record DeleteCardCommand(string Id) : IRequest;

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand>
{
    private readonly ICardsRepository _repository;

    public DeleteCardCommandHandler(ICardsRepository repository) => _repository = repository;

    // TODO: Authorization
    public async Task<Unit> Handle(DeleteCardCommand command, CancellationToken ct)
    {
        await _repository.Delete(command.Id, ct);
        return Unit.Value;
    }
}
