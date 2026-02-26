using AlertService.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AlertService.Services.AlertService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseMetricServer();
app.UseHttpMetrics();
app.MapControllers();
app.MapMetrics();

app.Run();