using ReserveHub.API.Extensions;
using ReserveHub.Infrastructure.Mappings;
using ReserveHub.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
.AddEnvironmentVariables();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
builder.Services.ConfigurePostgresSqlContext(builder.Configuration);builder.Services.AddSwaggerGen();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureIdentity();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

