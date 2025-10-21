using Serilog;
using Starter.Application;
using Starter.Infrastructure;
using Starter.WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddWebInfrastructure();
builder.Services.AddApplication();
builder.AddInfrastructure();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
await app.UseInfrastructureAsync();
app.UseWebInfrastructure();

Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

app.Run();
