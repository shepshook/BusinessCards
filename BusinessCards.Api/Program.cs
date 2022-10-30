using BusinessCards.Api;
using BusinessCards.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<CardsRepository>();
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(DbSettings.Section));
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

var app = builder.Build();

app.MapGet("cards",
    (IMediator mediator, CancellationToken ct) =>
        mediator.Send(new GetUserCardsQuery(), ct));

app.MapGet("cards/{id:length(24)}",
    (string id, IMediator mediator, CancellationToken ct) =>
        mediator.Send(new GetCardQuery(id), ct));

app.MapPost("cards", 
    (CreateCardCommand command, IMediator mediator, CancellationToken ct) => 
        mediator.Send(command, ct));

app.MapPut("cards/{id:length(24)}",
    (string id, UpdateCardCommand command, IMediator mediator, CancellationToken ct) =>
        mediator.Send(command with { Id = id }, ct));

app.MapDelete("cards/{id:length(24)}", 
    (string id, IMediator mediator, CancellationToken ct) => 
        mediator.Send(new DeleteCardCommand(id), ct));

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
    opt.RoutePrefix = "";
});

// app.UseHttpsRedirection();

app.Run();
