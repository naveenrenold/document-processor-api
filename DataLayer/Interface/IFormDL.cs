using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer.Interface
{
    public interface IFormDL
    {
        IEnumerable<Form> GetForm();
        bool PostForm();
    }
}