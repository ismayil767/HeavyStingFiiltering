using HeavyStringFiltering.Application;
using HeavyStringFiltering.Application.Settings;
using HeavyStringFiltering.Infrastructure;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.Configure<FilterSettings>(builder.Configuration.GetSection("FilterSettings"));
builder.Services.AddControllers();
builder.Services.AddHostedService<FilteringBackgroundService.FilteringBackgroundService>();
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR with assembly scanning for handlers


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHttpMetrics();

app.MapMetrics();

app.Run();
