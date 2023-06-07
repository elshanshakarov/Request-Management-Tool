using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Helpers
{
    public class FileHelperManager : IFileHelper
    {
        public string Upload(IFormFile file, string dir)
        {
            if (file != null || file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                //  var fileDir = "D:\\Elşən\\CIC\\Request\\Files";
                var filePath = $"{dir}\\{Guid.NewGuid().ToString()}{extension}";

                if (!Directory.Exists(Path.Combine(dir)))
                {
                    Directory.CreateDirectory(Path.Combine(dir));
                }

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush(); //clipboard-dan siler
                    return filePath;
                }
            }
            return null;
        }

        public void Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string Update(IFormFile file, string dir, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Upload(file, dir);
        }
    }
}
