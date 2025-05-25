using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DocumentProcessor.Model
{
    public class Attachment
    {
        [BindNever]
        public int AttachmentId { get; set; }
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? UploadedBy { get; set; }
        public string? UploadedOn { get; set; }
    }
}
