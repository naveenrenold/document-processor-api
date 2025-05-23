namespace DocumentProcessor.Model
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public int Id { get; set; }
        public int ActivityTypeId { get; set; }
        public string? Comments { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedOn { get; set; }
    }
}
