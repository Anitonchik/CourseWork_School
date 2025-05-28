using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SchoolWebApi.Services;
using SchoolContracts.AdapterContracts;
using SchoolWebApi.Adapters;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Implementations;
using SchoolDatabase;
using SchoolContracts.Infrastructure;
using SchoolContracts;
using SchoolContracts.BusinessLogicsContracts;
using SchoolBuisnessLogic.Implementations;
using Microsoft.OpenApi.Models;
using SchoolBusinessLogic.Implementations;
using SchoolBuisnessLogic.OfficePackage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin(); /*"http://localhost:5173"*/
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

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
builder.Services.AddTransient<IWorkerBuisnessLogicContract, WorkerBuisnessLogicContract>();
builder.Services.AddTransient<IAchievementBuisnessLogicContract, AchievementBuisnessLogicContract>();
builder.Services.AddTransient<ICircleBuisnessLogicContract, CircleBuisnessLogicContract>();
builder.Services.AddTransient<IInterestBuisnessLogicContract, InterestBuisnessLogicContract>();
builder.Services.AddTransient<ILessonBuisnessLogicContract, LessonBuisnessLogicContract>();
builder.Services.AddTransient<ILessonCircleBuisnessLogicContract, LessonCircleBuisnessLogicContract>();
builder.Services.AddTransient<ILessonInterestBuisnessLogicContract, LessonInterestBuisnessLogicContract>();
builder.Services.AddTransient<IMaterialBuisnessLogicContract, MaterialBuisnessLogicContract>();
builder.Services.AddTransient<IMedalBuisnessLogicContract, MedalBuisnessLogicContract>();

builder.Services.AddTransient<SchoolDbContext>();
builder.Services.AddTransient<IStorekeeperStorageContract, StorekeeperStorageContract>();
builder.Services.AddTransient<IWorkerStorageContract, WorkerStorageContract>();
builder.Services.AddTransient<IAchievementStorageContract, AchievementStorageContract>();
builder.Services.AddTransient<ICircleStorageContract, CircleStorageContract>();
builder.Services.AddTransient<ICircleMaterialStorageContract, CircleMaterialStorageContract>();
builder.Services.AddTransient<IInterestStorageContract, InterestStorageContract>();
builder.Services.AddTransient<ILessonStorageContract, LessonStorageContract>();
builder.Services.AddTransient<ILessonCircleStorageContract, LessonCircleStorageContract>();
builder.Services.AddTransient<ILessonInterestStorageContract, LessonInterestStorageContract>();
builder.Services.AddTransient<IMaterialStorageContract, MaterialStorageContract>();
builder.Services.AddTransient<IMedalStorageContract, MedalStorageContract>();
builder.Services.AddTransient<IReportContract, ReportContract>();

builder.Services.AddTransient<IStorekeeperAdapter, StorekeeperAdapter>();
builder.Services.AddTransient<IWorkerAdapter, WorkerAdapter>();
builder.Services.AddTransient<IAchievementAdapter, AchievementAdapter>();
builder.Services.AddTransient<ICircleAdapter, CircleAdapter>();
builder.Services.AddTransient<IInterestAdapter, InterestAdapter>();
builder.Services.AddTransient<ILessonAdapter, LessonAdapter>();
builder.Services.AddTransient<IMaterialAdapter, MaterialAdapter>();
builder.Services.AddTransient<IMedalAdapter, MedalAdapter>();
builder.Services.AddTransient<ILessonCircleAdapter, LessonCircleAdapter>();
builder.Services.AddTransient<ILessonInterestAdapter, LessonInterestAdapter>();

builder.Services.AddTransient<IReportAdapter, ReportAdapter>();
builder.Services.AddTransient<BaseWordBuilder, OpenXmlWordBuilder>();
builder.Services.AddTransient<BaseExcelBuilder, OpenXmlExcelBuilder>();
builder.Services.AddTransient<BasePdfBuilder, MigraDocPdfBuilder>();
builder.Services.AddTransient<IReportAdapter, ReportAdapter>();

builder.Services.AddScoped<JwtService>();


builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT Access Token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

