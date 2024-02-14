var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("reverseProxy"));

var app = builder.Build();

//var config = app.Configuration;

// app.UseHttpsRedirection();
//
// app.UseAuthentication();
//
// app.UseAuthorization();
//
// app.MapControllers();

app.MapGet("/", context => context.Response.WriteAsync("Confab API Gateway"));
app.MapReverseProxy();

app.Run();