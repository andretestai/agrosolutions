namespace IngestionService.Models
{
    public class SensorReading
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FieldId { get; set; }
        public double SoilHumidity { get; set; }
        public double Temperature { get; set; }
        public double Precipitation { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}
