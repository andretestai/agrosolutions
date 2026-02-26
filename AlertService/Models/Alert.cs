namespace AlertService.Models
{
    public class Alert
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FieldId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
