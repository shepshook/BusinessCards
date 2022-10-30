namespace BusinessCards.Api;

using System.Threading;
using System.Threading.Tasks;
using BusinessCards.Infrastructure;
using MediatR;

public record DeleteCardCommand(string Id) : IRequest;

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand>
{
    private readonly CardsRepository _repository;

    public DeleteCardCommandHandler(CardsRepository repository) => _repository = repository;

    // TODO: Authorization
    public async Task<Unit> Handle(DeleteCardCommand command, CancellationToken ct)
    {
        await _repository.Delete(command.Id, ct);
        return Unit.Value;
    }
}
