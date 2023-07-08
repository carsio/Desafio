using Core.Filters;
using Core.Repositories;
using Core.Repositories.Interfaces;
using Core.Services;
using Core.Services.Interfaces;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => {
    options.Filters.Add(new ExceptionFilter(builder.Environment));
    var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ExecutionTimeActionFilter>();
    options.Filters.Add(new ExecutionTimeActionFilter(logger));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

builder.Services.AddSingleton(x => {
    var cosmosDbConnectionString = builder.Configuration["ConnectionStrings:CosmosDBConnection"];
    var cosmosClient = new CosmosClient(cosmosDbConnectionString);
    var database = cosmosClient.CreateDatabaseIfNotExistsAsync("core").Result;
    var container = database.Database.CreateContainerIfNotExistsAsync("users", "/id").Result;
    return cosmosClient;
});

builder.Services.AddSingleton(x => {
    var serviceBusConnectionString = builder.Configuration["ConnectionStrings:ServiceBusConnection"];
    return new ServiceBusService(serviceBusConnectionString!);
    
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<JwtAuthorizationFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<PhoneNumberValidationService>();

app.Run();

public partial class Program { }
