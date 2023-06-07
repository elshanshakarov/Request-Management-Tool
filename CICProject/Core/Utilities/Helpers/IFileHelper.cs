using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Helpers
{
    public interface IFileHelper
    {
        string Upload(IFormFile file, string dir);
        string Update(IFormFile file, string dir, string filePath);
        void Delete(string filePath);
    }
}
