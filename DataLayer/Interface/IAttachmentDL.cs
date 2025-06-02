using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IAttachmentDL
    {
        public Task<IEnumerable<Attachment>> GetAttachment(QueryFilter filter);
        public Task<bool> PostAttachment(QueryFilter filter);
    }
}