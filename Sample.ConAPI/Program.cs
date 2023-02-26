using Sample.API.Extensions;
using Sample.API.Extensions.Endpoints;
using Sample.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = builder.Configuration;
AppConfig.Configuration = config;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();


builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.RegisterServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
app.ConfigureExceptionHandler(loggerFactory, app.Environment);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true));

app.UseAuthorization();

app.MapControllers();

app.MapCategoryEndpoints(loggerFactory);
app.MapCustomerEndpoints(loggerFactory);

app.Run();
