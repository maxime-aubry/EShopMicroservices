using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Assembly assembly = typeof(Program).Assembly;
string dbConnexionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(dbConnexionString);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnexionString);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler((IApplicationBuilder options) => {});

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
