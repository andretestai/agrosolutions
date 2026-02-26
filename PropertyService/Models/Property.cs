namespace PropertyService.Models
{
    public class Property
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double TotalAreaHectares { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Field> Fields { get; set; } = new();
    }
}
