using PropertyService.DTOs;
using PropertyService.Models;

namespace PropertyService.Services;

public class PropertyService
{
    private static readonly List<Property> _properties = new();

    public PropertyResponse Create(Guid ownerId, CreatePropertyRequest req)
    {
        var property = new Property
        {
            OwnerId = ownerId,
            Name = req.Name,
            Location = req.Location,
            TotalAreaHectares = req.TotalAreaHectares
        };

        _properties.Add(property);

        return ToResponse(property);
    }

    public List<PropertyResponse> GetByOwner(Guid ownerId)
    {
        return _properties
            .Where(p => p.OwnerId == ownerId)
            .Select(ToResponse)
            .ToList();
    }

    public PropertyResponse? GetById(Guid id, Guid ownerId)
    {
        var property = _properties
            .FirstOrDefault(p => p.Id == id && p.OwnerId == ownerId);

        return property is null ? null : ToResponse(property);
    }

    public FieldResponse? AddField(Guid propertyId, Guid ownerId, CreateFieldRequest req)
    {
        var property = _properties
            .FirstOrDefault(p => p.Id == propertyId && p.OwnerId == ownerId);

        if (property is null) return null;

        var field = new Field
        {
            PropertyId = propertyId,
            Name = req.Name,
            Crop = req.Crop,
            AreaHectares = req.AreaHectares
        };

        property.Fields.Add(field);

        return ToFieldResponse(field);
    }

    public List<FieldResponse> GetFields(Guid propertyId, Guid ownerId)
    {
        var property = _properties
            .FirstOrDefault(p => p.Id == propertyId && p.OwnerId == ownerId);

        if (property is null) return new();

        return property.Fields.Select(ToFieldResponse).ToList();
    }

    // Mappers
    private static PropertyResponse ToResponse(Property p) => new(
        p.Id, p.Name, p.Location, p.TotalAreaHectares, p.CreatedAt,
        p.Fields.Select(ToFieldResponse).ToList()
    );

    private static FieldResponse ToFieldResponse(Field f) => new(
        f.Id, f.PropertyId, f.Name, f.Crop, f.AreaHectares, f.Status, f.CreatedAt
    );
}