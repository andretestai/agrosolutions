using AlertService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton para manter a lista em memória
builder.Services.AddSingleton<AlertService.Services.AlertService>();

// Background service que consome a fila do RabbitMQ
builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();