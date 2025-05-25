using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IFormDL
    {
        Task<IEnumerable<Form>> GetForm();
        Task<int> PostForm(Form request, List<Attachment> attachments);
    }
}