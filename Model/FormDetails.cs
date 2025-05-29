namespace DocumentProcessor.Model
{
    public class FormDetails : BaseFilter
    {
        public FormDetails() : base("Id")
        {            
        }
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int ProcessId { get; set; }
        public string? Location { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
