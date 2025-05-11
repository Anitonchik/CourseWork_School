using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using SchoolWebApi.Services;
using SchoolContracts.AdapterContracts;
using SchoolWebApi.Controllers;
using SchoolWebApi.Adapters;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Implementations;
using SchoolDatabase;
using SchoolContracts.Infrastructure;
using SchoolContracts;
using SchoolContracts.BusinessLogicsContracts;
using SchoolBuisnessLogic.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IConnectionString, ConnectionString>();

builder.Services.AddTransient<IStorekeeperBuisnessLogicContract, StorekeeperBuisnessLogicContract>();

builder.Services.AddTransient<SchoolDbContext>();
builder.Services.AddTransient<IStorekeeperStorageContract, StorekeeperStorageContract>();

builder.Services.AddTransient<IStorekeeperAdapter, UserStorekeeperAdapter>();

builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
