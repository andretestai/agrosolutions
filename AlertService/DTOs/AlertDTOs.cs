namespace AlertService.DTOs;

public record SensorDataEvent(
    Guid FieldId,
    double SoilHumidity,
    double Temperature,
    double Precipitation,
    DateTime RecordedAt
);

public record AlertResponse(
    Guid Id,
    Guid FieldId,
    string Type,
    string Message,
    bool IsActive,
    DateTime CreatedAt
);