namespace DocumentProcessor.Model
{
    public class FormResponse
    {
        public int Id { get; set; }
        public required string  TypeName { get; set; }
        public required string StatusName { get; set; }
        public required string ProcessName { get; set; }
        public int ProcessId { get; set; }
        public string? LocationName { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
