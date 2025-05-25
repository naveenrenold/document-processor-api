using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IProcessDL
    {
        Task<IEnumerable<Process>> GetProcess();
    }
}