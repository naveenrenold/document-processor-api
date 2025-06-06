namespace DocumentProcessor.Model
{
    public class ActivityResponse
    {
        public int ActivityId { get; set; }
        public int Id { get; set; }
        public string? ActivityTypeName { get; set; }
        public string? Comments { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedOn { get; set; }
        public string? Field { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
