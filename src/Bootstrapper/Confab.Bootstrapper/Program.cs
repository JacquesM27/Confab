using Confab.Bootstrapper;
using Confab.Shared.Infrastructure;
using Confab.Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);

// Load all configuration files. 
builder.Host.ConfigureModules();

var assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
var modules = ModuleLoader.LoadModules(assemblies);

// Add services to the container.
builder.Services
    .AddInfrastructure(assemblies, modules);


foreach (var module in modules)
{
    module.Register(builder.Services);
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseInfrastructure();

foreach (var module in modules)
{
    module.Use(app);
}
app.Logger.LogInformation($"Modules: {string.Join(", ", modules.Select(x => x.Name))} loaded.");

assemblies.Clear();
modules.Clear();

app.MapControllers();
app.MapModuleInfo();

app.Run();

// All integration test projects will have a reference to Confab.Shared.Tests, so I don't want to add InternalsVisibleTo
public partial class Program { }
