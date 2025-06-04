using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IFormDL
    {
        Task<IEnumerable<FormResponse>> GetForm(QueryFilter<FormResponse> filter);
        Task<int> PostForm(Form request, IFormFileCollection? attachments, List<int> deleteAttachments);
    }
}