using IngestionService.DTOs;
using IngestionService.Models;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace IngestionService.Services;

public class IngestionService
{
    private static readonly List<SensorReading> _readings = new();
    private readonly IConfiguration _config;

    public IngestionService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<SensorReadingResponse> AddAsync(SensorReadingRequest req)
    {
        var reading = new SensorReading
        {
            FieldId = req.FieldId,
            SoilHumidity = req.SoilHumidity,
            Temperature = req.Temperature,
            Precipitation = req.Precipitation
        };

        _readings.Add(reading);

        // Publica no RabbitMQ
        await PublishAsync(new SensorDataEvent(
            reading.FieldId,
            reading.SoilHumidity,
            reading.Temperature,
            reading.Precipitation,
            reading.RecordedAt
        ));

        return ToResponse(reading);
    }

    public List<SensorReadingResponse> GetByField(Guid fieldId)
    {
        return _readings
            .Where(r => r.FieldId == fieldId)
            .OrderByDescending(r => r.RecordedAt)
            .Select(ToResponse)
            .ToList();
    }

    private async Task PublishAsync(SensorDataEvent evt)
    {
        try
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

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "sensor.data",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var json = JsonSerializer.Serialize(evt);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "sensor.data",
                body: body
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RabbitMQ] Erro ao publicar: {ex.Message}");
        }
    }

    private static SensorReadingResponse ToResponse(SensorReading r) => new(
        r.Id,
        r.FieldId,
        r.SoilHumidity,
        r.Temperature,
        r.Precipitation,
        r.RecordedAt
    );
}