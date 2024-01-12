using Confab.Bootstrapper;
using Confab.Modules.Conferences.Api;
using Confab.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddInfrastructure();

var assemblies = ModuleLoader.LoadAssemblies();
var modules = ModuleLoader.LoadModules(assemblies);

foreach (var module in modules)
{
    module.Register(builder.Services);
}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInfrastructure();

foreach (var module in modules)
{
    module.Use(app);
}
app.Logger.LogInformation($"Modules: {string.Join(", ", modules.Select(x => x.Name))} loaded.");

assemblies.Clear();
modules.Clear();

app.MapControllers();

app.Run();
