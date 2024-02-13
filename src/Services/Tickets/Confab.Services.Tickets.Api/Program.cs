using Confab.Services.Tickets.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();