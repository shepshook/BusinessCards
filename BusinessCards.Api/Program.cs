using BusinessCards.Api;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.MapEndpoints();

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
