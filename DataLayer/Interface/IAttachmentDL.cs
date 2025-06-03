using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IAttachmentDL
    {
        public Task<IEnumerable<Attachment>> GetAttachment(QueryFilter filter, bool? downloadAttachment = true);
        public Task<bool> PostAttachment(QueryFilter filter);
    }
}