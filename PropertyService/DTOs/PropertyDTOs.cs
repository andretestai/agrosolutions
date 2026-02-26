namespace PropertyService.DTOs;

public record CreatePropertyRequest(string Name, string Location, double TotalAreaHectares);

public record PropertyResponse(
    Guid Id,
    string Name,
    string Location,
    double TotalAreaHectares,
    DateTime CreatedAt,
    List<FieldResponse> Fields
);

public record CreateFieldRequest(string Name, string Crop, double AreaHectares);

public record FieldResponse(
    Guid Id,
    Guid PropertyId,
    string Name,
    string Crop,
    double AreaHectares,
    string Status,
    DateTime CreatedAt
);