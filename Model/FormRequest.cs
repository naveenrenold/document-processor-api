namespace DocumentProcessor.Model
{
    public class FormRequest 
    {
        public required Form Form { get; set; }
        public List<Attachment>? Attachments { get; set; }
    }
}
