using Starter.Application;
using Starter.Infrastructure;
using Starter.WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddWebInfrastructure();
builder.Services.AddApplication();
builder.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
await app.UseInfrastructureAsync();
app.UseWebInfrastructure();

app.Run();
