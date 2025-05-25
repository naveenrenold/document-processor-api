namespace DocumentProcessor.Model
{
    public class ValidationModel
    {
        public string? Message { get; set; }
        //public bool IsValid { get { return Message == null; }}

        public ValidationModel(string errors)
        {
            Message = errors;
        }

    }
}
