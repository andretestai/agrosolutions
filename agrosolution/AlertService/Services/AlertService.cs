using AlertService.DTOs;
using AlertService.Models;

namespace AlertService.Services;

public class AlertService
{
    private static readonly List<Alert> _alerts = new();

    private static readonly Dictionary<Guid, List<SensorDataEvent>> _readings = new();

    public void ProcessSensorData(SensorDataEvent data)
    {
        if (!_readings.ContainsKey(data.FieldId))
            _readings[data.FieldId] = new();

        _readings[data.FieldId].Add(data);

        CheckDroughtAlert(data.FieldId);
        CheckPlagueAlert(data);
    }

    public List<AlertResponse> GetByField(Guid fieldId)
    {
        return _alerts
            .Where(a => a.FieldId == fieldId && a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
            .Select(ToResponse)
            .ToList();
    }

    public List<AlertResponse> GetAll()
    {
        return _alerts
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
            .Select(ToResponse)
            .ToList();
    }

    private void CheckDroughtAlert(Guid fieldId)
    {
        var readings = _readings[fieldId];

        var last24h = readings
            .Where(r => r.RecordedAt >= DateTime.UtcNow.AddHours(-24))
            .ToList();

        if (last24h.Count > 0 && last24h.All(r => r.SoilHumidity < 30))
        {
            var jaExiste = _alerts.Any(a =>
                a.FieldId == fieldId &&
                a.Type == "Alerta de Seca" &&
                a.IsActive);

            if (!jaExiste)
            {
                _alerts.Add(new Alert
                {
                    FieldId = fieldId,
                    Type = "Alerta de Seca",
                    Message = $"Umidade do solo abaixo de 30% por mais de 24 horas no talhão {fieldId}."
                });
            }
        }
    }

    private void CheckPlagueAlert(SensorDataEvent data)
    {
        if (data.Temperature > 40)
        {
            var jaExiste = _alerts.Any(a =>
                a.FieldId == data.FieldId &&
                a.Type == "Risco de Praga" &&
                a.IsActive);

            if (!jaExiste)
            {
                _alerts.Add(new Alert
                {
                    FieldId = data.FieldId,
                    Type = "Risco de Praga",
                    Message = $"Temperatura acima de 40°C detectada no talhão {data.FieldId}."
                });
            }
        }
    }

    private static AlertResponse ToResponse(Alert a) => new(
        a.Id,
        a.FieldId,
        a.Type,
        a.Message,
        a.IsActive,
        a.CreatedAt
    );
}