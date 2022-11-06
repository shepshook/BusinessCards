using BusinessCards.Api;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.MapGet("cards",
    (IMediator mediator, CancellationToken ct, HttpContext context) => 
        mediator.Send(new GetUserCardsQuery(), ct))
        .RequireAuthorization();

app.MapGet("cards/{id:length(24)}",
    (string id, IMediator mediator, CancellationToken ct) =>
        mediator.Send(new GetCardQuery(id), ct)).RequireAuthorization();

app.MapPost("cards",
    (CreateCardCommand command, IMediator mediator, CancellationToken ct) =>
        mediator.Send(command, ct))
        .RequireAuthorization();

app.MapPut("cards/{id:length(24)}",
    (string id, UpdateCardCommand command, IMediator mediator, CancellationToken ct) =>
        mediator.Send(command with { Id = id }, ct))
        .RequireAuthorization();

app.MapDelete("cards/{id:length(24)}",
    (string id, IMediator mediator, CancellationToken ct) =>
        mediator.Send(new DeleteCardCommand(id), ct))
        .RequireAuthorization();

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
    opt.RoutePrefix = "";
});
app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();

app.Run();
