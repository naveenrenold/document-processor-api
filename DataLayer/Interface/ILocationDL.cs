using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface ILocationDL
    {
        Task<IEnumerable<Location>> GetLocation();
    }
}