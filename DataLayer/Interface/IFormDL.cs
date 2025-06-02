using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IFormDL
    {
        Task<IEnumerable<FormResponse>> GetForm(QueryFilter filter);
        Task<int> PostForm(Form request, IFormFileCollection? attachments);
    }
}