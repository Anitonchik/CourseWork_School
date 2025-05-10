using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddAuthentication("Bearer")  // добавление сервисов аутентификации
    .AddJwtBearer();      // подключение аутентификации с помощью jwt-токенов
builder.Services.AddAuthorization();            // добавление сервисов авторизации

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
