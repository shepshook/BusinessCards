using MediatR;

namespace BusinessCards.Api;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("cards",
            (IMediator mediator, CancellationToken ct) => mediator.Send(new GetUserCardsQuery(), ct))
            .RequireAuthorization();

        app.MapGet("cards/{id:length(24)}",
            (string id, IMediator mediator, CancellationToken ct) => mediator.Send(new GetCardQuery(id), ct))
            .RequireAuthorization();

        app.MapPost("cards",
            (CreateCardCommand command, IMediator mediator, CancellationToken ct) => mediator.Send(command, ct))
            .RequireAuthorization();

        app.MapPut("cards/{id:length(24)}",
            (string id, UpdateCardCommand command, IMediator mediator, CancellationToken ct) => mediator.Send(command with { Id = id }, ct))
            .RequireAuthorization();

        app.MapDelete("cards/{id:length(24)}",
            (string id, IMediator mediator, CancellationToken ct) => mediator.Send(new DeleteCardCommand(id), ct))
            .RequireAuthorization();
    }
}
