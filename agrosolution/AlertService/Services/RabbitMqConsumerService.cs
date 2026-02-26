using System.Text;
using System.Text.Json;
using AlertService.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AlertService.Services;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly AlertService _alertService;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqConsumerService(IConfiguration config, AlertService alertService)
    {
        _config = config;
        _alertService = alertService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var host = _config["RabbitMQ:Host"] ?? "localhost";
        var user = _config["RabbitMQ:User"] ?? "guest";
        var pass = _config["RabbitMQ:Pass"] ?? "guest";

        var factory = new ConnectionFactory
        {
            HostName = host,
            UserName = user,
            Password = pass
        };

        // Aguarda o RabbitMQ ficar disponível
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync(
                    queue: "sensor.data",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    cancellationToken: stoppingToken
                );

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    var evt = JsonSerializer.Deserialize<SensorDataEvent>(json);
                    if (evt is not null)
                        _alertService.ProcessSensorData(evt);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await _channel.BasicConsumeAsync(
                    queue: "sensor.data",
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken
                );

                Console.WriteLine("[RabbitMQ] Consumidor conectado e aguardando mensagens.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RabbitMQ] Aguardando conexão... {ex.Message}");
                await Task.Delay(3000, stoppingToken);
            }
        }

        // Mantém o serviço rodando
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}