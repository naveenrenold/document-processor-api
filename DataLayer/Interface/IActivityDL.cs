using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IActivityDL
    {
        Task<IEnumerable<ActivityResponse>> GetActivity(QueryFilter<ActivityResponse> filter);
    }
}