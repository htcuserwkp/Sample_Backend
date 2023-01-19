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

// Add services to the container.
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.RegisterServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true));

app.UseAuthorization();

app.MapControllers();

app.MapCategoryEndpoints();
app.MapCustomerEndpoints();

app.Run();
