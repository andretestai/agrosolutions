namespace PropertyService.Models
{
    public class Field
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PropertyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Crop { get; set; } = string.Empty;
        public double AreaHectares { get; set; }
        public string Status { get; set; } = "Normal";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
