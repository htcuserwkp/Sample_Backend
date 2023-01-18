using Sample.API;
using Sample.API.EndpointExtensions;
using Sample.Common;

var builder = WebApplication.CreateBuilder(args);


IConfiguration config = builder.Configuration;
AppConfig.Configuration = config;
// Add services to the container.

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
