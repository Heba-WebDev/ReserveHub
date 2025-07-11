using ReserveHub.API.Extensions;
using ReserveHub.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigurePostgresSqlContext(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();

