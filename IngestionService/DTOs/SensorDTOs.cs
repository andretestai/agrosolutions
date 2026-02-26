namespace IngestionService.DTOs;

public record SensorReadingRequest(
    Guid FieldId,
    double SoilHumidity,
    double Temperature,
    double Precipitation
);

public record SensorReadingResponse(
    Guid Id,
    Guid FieldId,
    double SoilHumidity,
    double Temperature,
    double Precipitation,
    DateTime RecordedAt
);

public record SensorDataEvent(
    Guid FieldId,
    double SoilHumidity,
    double Temperature,
    double Precipitation,
    DateTime RecordedAt
);