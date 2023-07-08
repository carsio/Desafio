using Notification.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(options =>
{
    var accountSid = builder.Configuration["Twilio:AccountSid"];
    var authToken = builder.Configuration["Twilio:AuthToken"];
    var fromPhoneNumber = builder.Configuration["Twilio:FromPhoneNumber"];
    return new SmsService(accountSid!, authToken!, fromPhoneNumber!);
});

builder.Services.AddSingleton(options =>
{
    var smsService = options.GetRequiredService<SmsService>();
    var connectionString = builder.Configuration["ConnectionStrings:ServiceBusConnection"];
    return new ServiceBusConsumerService(connectionString!, smsService);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var serviceBusConsumerService = app.Services.GetRequiredService<ServiceBusConsumerService>();
serviceBusConsumerService.StartProcessingMessages();

app.Run();
