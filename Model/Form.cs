using System.ComponentModel.DataAnnotations;

namespace DocumentProcessor.Model
{
    public class Form
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int ProcessId { get; set; }
        public string? Location { get; set; }
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
